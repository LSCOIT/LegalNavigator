namespace Access2Justice.Shared.Admin
{
    public enum JsonUploadError
    {
        UnhandledError = 101,
        ModelStateInvalid = 102,
        FileExtensionIsInvalid = 103,
        NameIsIncorrect = 104,
        MissingTopic = 105,
        MainTemplateDbSaveError = 106,
        CuratedTemplateDbSaveError = 107,
        FilesNotFound = 108,
        RootNodeIsMissing = 109,
        ChildrenNodeIsMissing = 110,
        CouldNotRetrieveTemplateOrder = 111
    }
}