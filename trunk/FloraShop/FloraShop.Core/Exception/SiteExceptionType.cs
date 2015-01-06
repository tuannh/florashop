using System;

namespace FloraShop.Core
{
    public enum SiteExceptionType
    {
        DataProvider,
        AdministrationAccessDenied,
        PostEditAccessDenied,
        ModerateAccessDenied,
        PostDuplicate,
        FileNotFound,
        CreateUser,
        UserAccountCreated,
        UserAccountPending,
        UserAccountCreatedAuto,
        UserAccountDisapproved,
        UserAccountBanned,
        UserProfileUpdated,
        UserAccountInvalid,
        UserNotFound,
        UserPasswordChangeSuccess,
        UserPasswordChangeFailed,
        UserInvalidCredentials,
        UserSearchNotFound,
        UserUnknownLoginError,
        EmailUnableToSend,
        UserAccountRegistrationDisabled,
        UserLoginDisabled,
        AccessDenied,
        PostAccessDenied,
        EmailTemplateNotFound,
        SearchUnknownError,
        SkinNotSet,
        SkinNotFound,
        ReturnURLRequired,
        SearchNoResults,
        GeneralAccessDenied,
        EmailSentToUser,
        UserPasswordAnswerChangeSuccess,
        UserPasswordAnswerChangeFailed,
        RoleNotFound,
        RoleUpdated,
        RoleDeleted,
        RoleOperationUnavailable,
        ResourceNotFound,
        PermissionApplicationUnknown,
        RedirectFailure,
        UnRegisteredSite,
        SiteSettingsInvalidXML,
        UserPasswordResetSuccess,
        UserPasswordLinkSentSuccess,
        UnKnownProvider,
        UserAlreadyLoggedIn,
        InvalidLicense,
        LicenseAccessError,
        UserAccountRequiresValidInvitation,
        FileTypeNotValid,
        SiteFileQuotaExceeded,
        SiteFileStorageNotAllowed,
        LoadModuleFailed,
        UpdateModuleFail,
        ImportLanguageFail,
        ExportLanguageFail,

        ApplicationStart,
        ApplicationStop,

        InsertNewObject,
        UpdateObject,
        InsertOrUpdateObject,
        DeleteObject,
        DeleteListObject,

        BundleNameDoesNotExit,

        EncryptPasswordError,

        ScheduleJobExp,

        UnknownError
    }
}