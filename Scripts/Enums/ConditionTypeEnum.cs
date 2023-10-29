namespace Assets.Scripts.Enums
{
    [System.Serializable]
    public enum ConditionTypeEnum
    {
        StartedQuest,
        EndQuest,
        AnySaid,
        //SomeoneSaid, //Problématique car il a besoin de deux param
        PossessItem,
        PossessMoney,
        Location,
        Event
    }
}