using Assets.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DialogueManager : SingletonGameObject<DialogueManager>
{
    public static event Action<string[]> OnDiscuss;

    public AudioSource dialogAudioSource;
    public PauseManager pauseManager;
    public GameObject dialogueButtonPrefab;
    [SerializeField] Sprite thinkingSprite;
    [SerializeField] Sprite outLoudSprite;

    public List<DialogEmotion> _emotions;

    DialogueContainer _dialogueContainer;
    Text textArea;

    string _currentText = string.Empty;
    Action OnEndAnimation;

    void Start()
    {
        dialogAudioSource.volume = MainGameManager._soundManager.SoundVolume;

        foreach (DialogEmotionType emotion in Enum.GetValues(typeof(DialogEmotionType)))
        {
            if (!_emotions.Exists(x => x.Type == emotion))
            {
                _emotions.Add(new DialogEmotion() { Type = emotion });
            }
        }
    }

    public void StartDialogue(DialogueContainer dialogueContainer, Action OnLoaded = null)
    {
        _dialogueContainer = dialogueContainer;

        ShowPauseInterface(OnLoaded);
    }

    public void StartDialogue(DialogueReference dialogueRef)
    {
        _dialogueContainer = dialogueRef.DialogueContainer;

        ShowPauseInterface();
    }

    void ShowPauseInterface(Action OnLoaded = null)
    {
        pauseManager.ShowPausedInterface(new PauseParameter()
        {
            InterfaceName = "DialogueScene",
            OnPauseProcessed = () =>
            {
                OnLoaded?.Invoke();

                textArea = FindGameObjectHelper.FindByName("Dialogue Text").GetComponent<Text>();

                BaseNodeData node = GraphHelper.GetFirstNode(_dialogueContainer);
                NextNode(node);
            },
            PlaySound = false
        });
    }

    void NextNode(BaseNodeData node)
    {
        if (node is DialogueNodeData)
        {
            DisplayDialogue(node as DialogueNodeData);
        }
        else if (node is EventNodeData)
        {
            ProcessEvent(node as EventNodeData);
        }
        else
        {
            //Error
        }
    }

    void DisplayDialogue(DialogueNodeData node)
    {
        ClearButtonsLayout();

        GameObject dialogueBox = FindGameObjectHelper.FindByName("Dialog Box");
        dialogueBox.GetComponent<Image>().sprite = (node.Type == DialogueTypeEnum.talk) ? outLoudSprite : thinkingSprite;
        if (node.Type == DialogueTypeEnum.think) dialogueBox.GetComponent<Image>().type = Image.Type.Tiled;

        GameObject pnjNameLeft = FindGameObjectHelper.FindByName("PNJ Name Left");
        pnjNameLeft.SetActive(node.Side == DialogueNodeSide.left);
        pnjNameLeft.GetComponent<Text>().text = node.Pnj.Name + ":";

        GameObject pnjSpriteLeft = FindGameObjectHelper.FindByName("PNJ Sprite Left");
        pnjSpriteLeft.SetActive(node.Side == DialogueNodeSide.left && node.Type != DialogueTypeEnum.think);
        pnjSpriteLeft.GetComponent<Image>().sprite = node.Pnj.Sprite;

        GameObject pnjNameRight = FindGameObjectHelper.FindByName("PNJ Name Right");
        pnjNameRight.SetActive(node.Side == DialogueNodeSide.right && node.Type != DialogueTypeEnum.think);
        pnjNameRight.GetComponent<Text>().text = node.Pnj.Name + ":";

        GameObject pnjSpriteRight = FindGameObjectHelper.FindByName("PNJ Sprite Right");
        pnjSpriteRight.SetActive(node.Side == DialogueNodeSide.right);
        pnjSpriteRight.GetComponent<Image>().sprite = node.Pnj.Sprite;

        OnDiscuss?.Invoke(new string[] { node.DialogueCode });

        Action showButtons = () =>
        {
            List<NodeLinkData> connections = GraphHelper.GetConnections(_dialogueContainer, node);
            if (node.Outputs.Length > 0)
            {
                foreach (string output in node.Outputs)
                {
                    if (connections.Exists(x => x.PortName == output))
                    {
                        InstanciateChoiceButton(
                            (output == string.Empty) ? ">" : output,
                            () => {
                                node.Pnj.AddSaid(node.DialogueCode);
                                BaseNodeData nextNode = GraphHelper.GetNodeByGuid(_dialogueContainer, connections.Find(x => x.PortName == output).TargetNodeGuid);
                                NextNode(nextNode);
                            }
                        );
                    }
                    else
                    {
                        InstanciateChoiceButton(
                            ((output == string.Empty) ? ">" : output),
                            () => {
                                node.Pnj.AddSaid(node.DialogueCode);
                                pauseManager.Resume();
                            });
                    }

                }
            }
            else
            {
                InstanciateChoiceButton(">", () =>
                {
                    node.Pnj.AddSaid(node.DialogueCode);
                    pauseManager.Resume();
                });
            }
        };

        _currentText = node.DialogueText;
        OnEndAnimation = showButtons;
        StartCoroutine(TextAnimationByLetter(node.DialogueText, node.Pnj.Voices, _emotions.Find(x => x.Type == node.Emotion), showButtons));
    }

    void InstanciateChoiceButton(string portName, Action clickAction)
    {
        //tester Instantiate(prefab, parent);
        GameObject choiceButton = Instantiate(dialogueButtonPrefab, Vector3.zero, Quaternion.identity);
        Vector3 scale = choiceButton.transform.localScale;

        GameObject dialogueButtonsLayout = FindGameObjectHelper.FindByName("Dialogue Buttons Layout");
        choiceButton.transform.SetParent(dialogueButtonsLayout.transform, true);
        choiceButton.transform.localScale = scale;

        choiceButton.GetComponentInChildren<Text>().text = portName;
        choiceButton.GetComponent<Button>().onClick.AddListener(() => { clickAction?.Invoke(); });
    }

    void ProcessEvent(EventNodeData node)
    {
        bool error = false;

        switch (node.Type)
        {
            case EventTypeEnum.StartQuest: MainGameManager._questManager.AddQuest(node.Param); break;
            case EventTypeEnum.EndQuest: MainGameManager._questManager.GetQuest(node.Param).IsCompleted = true; break;
            case EventTypeEnum.AddItem: if (!MainGameManager._inventoryManager.AddItem(node.Param)) error = true; break;
            case EventTypeEnum.RemoveItem: MainGameManager._inventoryManager.RemoveItem(node.Param); break;
            case EventTypeEnum.AddMoney: MainGameManager._inventoryManager.AddMoney(int.Parse(node.Param)); break;
            case EventTypeEnum.RemoveMoney: if (!MainGameManager._inventoryManager.RemoveMoney(int.Parse(node.Param))) { error = true; }; break;
            //case EventTypeEnum.SetDialogueSaid: break;
            case EventTypeEnum.StartCinematic: break;
            default: break;
        }

        var connections = GraphHelper.GetConnections(_dialogueContainer, node);
        if (connections.Count > 0 && !error)
        {
            BaseNodeData nextNode = GraphHelper.GetNodeByGuid(_dialogueContainer, connections[0].TargetNodeGuid);
            NextNode(nextNode);
        }
        else
        {
            pauseManager.Resume();
        }
    }

    void ClearButtonsLayout()
    {
        GameObject dialogueButtonsLayout = FindGameObjectHelper.FindByName("Dialogue Buttons Layout");
        foreach (Transform child in dialogueButtonsLayout.transform)
        {
            Destroy(child.gameObject);
        }

        OnEndAnimation = null;
    }

    readonly char[] alphabet = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
    readonly char[] pronounced = { 'b', 'c', 'd', 'f', 'g', 'j', 'k', 'l', 'm', 'n', 'p', 'q', 'r', 's', 't', 'v', 'w', 'x', 'z', 'ç' };
    //readonly char[] unpronounced = { 'a', 'e', 'i', 'o', 'u', 'y', 'h' };
    readonly char questionMark = '?';
    readonly char affirmativeMark = '!';
    readonly string suspenseMark = "...";

    public void Accelerate()
    {
        StopAllCoroutines();
        textArea.text = _currentText;
        OnEndAnimation?.Invoke();
        OnEndAnimation = null;
    }

    public void Resume()
    {
        StopAllCoroutines();
        FindAnyObjectByType<PauseManager>().Resume();
    }

    IEnumerator TextAnimationByLetter(string text, AudioClip[] voices, DialogEmotion emotion, Action OnEndAnimation)
    {
        textArea.text = "";

        string[] words = text.Split(new Char[] { ' ', '\n', '-', });

        bool lastWasConsonnant = false;

        foreach (string word in words)
        {
            var loweredCharWord = word.ToLower().ToCharArray();
            var charWord = word.ToCharArray();
            for (int i = 0; i < charWord.Length; i++)
            {
                textArea.text += charWord[i];

                if (voices.Length > 0 && alphabet.Contains(loweredCharWord[i]))
                    ManageVoice(i, ref lastWasConsonnant, loweredCharWord, voices, emotion);

                yield return new WaitForSecondsRealtime(emotion.Speed);
            }

            textArea.text += " ";

            lastWasConsonnant = false;

            yield return new WaitForSecondsRealtime(emotion.Speed * 2);
        }

        OnEndAnimation?.Invoke();
    }

    void ManageVoice(int index, ref bool lastWasConsonnant, char[] word, AudioClip[] voices, DialogEmotion emotion)
    {
        word = word.Where(x => x != ',' && x != '.').ToArray();

        if (index >= word.Length - 1)
            return;

        //float specialPitch = SpecialPitch(word);

        bool isPronunced = pronounced.Contains(word[index]);

        if (index == 0)
        {
            PlayVoiceWithEmotion(word[index], voices, emotion);

            if (isPronunced)
                lastWasConsonnant = true;

            return;
        }
        
        if (isPronunced)
        {
            if (!lastWasConsonnant)
            {
                PlayVoiceWithEmotion(word[index], voices, emotion);
            }

            lastWasConsonnant = true;
        }
        else
        {
            lastWasConsonnant = false;
        }
    }

    async void PlayVoiceWithEmotion(char letter, AudioClip[] voices, DialogEmotion emotion, float specialPitch = 0)
    {
        AudioClip voice;
        
        if (specialPitch == 0)
        {
            int hashCode = letter.GetHashCode();
            int predictableIndex = hashCode % voices.Length;

            voice = voices[predictableIndex];

            dialogAudioSource.pitch = Random.Range(emotion.MinPitch, emotion.MaxPitch);

            int minPitchInt = (int)(emotion.MinPitch * 100);
            int maxPitchInt = (int)(emotion.MaxPitch * 100);
            int pitchRangeInt = maxPitchInt - minPitchInt;
            // cannot divide by 0, so if there is no range then skip the selection
            if (pitchRangeInt != 0)
            {
                int predictablePitchInt = (hashCode % pitchRangeInt) + minPitchInt;
                float predictablePitch = predictablePitchInt / 100f;
                dialogAudioSource.pitch = predictablePitch;
            }
            else
            {
                dialogAudioSource.pitch = emotion.MinPitch;
            }
        }
        else
        {
            dialogAudioSource.pitch = specialPitch;
            voice = voices[Random.Range(0, voices.Length)];
        }

        dialogAudioSource.Stop();
        dialogAudioSource.PlayOneShot(voice);
    }

    float SpecialPitch(char[] word)
    {
        if (word.LastOrDefault() == '?') return 1;

        if (word.LastOrDefault() == '!') return 1;

        if (word[word.Length - 1] == '.'
            && word[word.Length - 2] == '.'
            && word[word.Length - 3] == '.')
        {
            return 1;
        }

        return 0;
    }

}