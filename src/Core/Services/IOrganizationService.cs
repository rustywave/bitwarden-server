﻿using Bit.Core.Entities;
using Bit.Core.Enums;
using Bit.Core.Models.Business;
using Bit.Core.Models.Data;

namespace Bit.Core.Services
{
    public interface IOrganizationService
    {
        Task ReplacePaymentMethodAsync(Guid organizationId, string paymentToken, PaymentMethodType paymentMethodType,
            TaxInfo taxInfo);
        Task CancelSubscriptionAsync(Guid organizationId, bool? endOfPeriod = null);
        Task ReinstateSubscriptionAsync(Guid organizationId);
        Task<Tuple<bool, string>> UpgradePlanAsync(Guid organizationId, OrganizationUpgrade upgrade);
        Task<string> AdjustStorageAsync(Guid organizationId, short storageAdjustmentGb);
        Task UpdateSubscription(Guid organizationId, int seatAdjustment, int? maxAutoscaleSeats);
        Task AutoAddSeatsAsync(Organization organization, int seatsToAdd, DateTime? prorationDate = null);
        Task<string> AdjustSeatsAsync(Guid organizationId, int seatAdjustment, DateTime? prorationDate = null);
        Task VerifyBankAsync(Guid organizationId, int amount1, int amount2);
        Task<Tuple<Organization, OrganizationUser>> SignUpAsync(OrganizationSignup organizationSignup, bool provider = false);
        Task<Tuple<Organization, OrganizationUser>> SignUpAsync(OrganizationLicense license, User owner,
            string ownerKey, string collectionName, string publicKey, string privateKey);
        Task UpdateLicenseAsync(Guid organizationId, OrganizationLicense license);
        Task DeleteAsync(Organization organization);
        Task EnableAsync(Guid organizationId, DateTime? expirationDate);
        Task DisableAsync(Guid organizationId, DateTime? expirationDate);
        Task UpdateExpirationDateAsync(Guid organizationId, DateTime? expirationDate);
        Task EnableAsync(Guid organizationId);
        Task UpdateAsync(Organization organization, bool updateBilling = false);
        Task UpdateTwoFactorProviderAsync(Organization organization, TwoFactorProviderType type);
        Task DisableTwoFactorProviderAsync(Organization organization, TwoFactorProviderType type);
        Task<List<OrganizationUser>> InviteUsersAsync(Guid organizationId, Guid? invitingUserId,
            IEnumerable<(OrganizationUserInvite invite, string externalId)> invites);
        Task<OrganizationUser> InviteUserAsync(Guid organizationId, Guid? invitingUserId, string email,
            OrganizationUserType type, bool accessAll, string externalId, IEnumerable<SelectionReadOnly> collections);
        Task<IEnumerable<Tuple<OrganizationUser, string>>> ResendInvitesAsync(Guid organizationId, Guid? invitingUserId, IEnumerable<Guid> organizationUsersId);
        Task ResendInviteAsync(Guid organizationId, Guid? invitingUserId, Guid organizationUserId);
        Task<OrganizationUser> AcceptUserAsync(Guid organizationUserId, User user, string token,
            IUserService userService);
        Task<OrganizationUser> AcceptUserAsync(string orgIdentifier, User user, IUserService userService);
        Task<OrganizationUser> ConfirmUserAsync(Guid organizationId, Guid organizationUserId, string key,
            Guid confirmingUserId, IUserService userService);
        Task<List<Tuple<OrganizationUser, string>>> ConfirmUsersAsync(Guid organizationId, Dictionary<Guid, string> keys,
            Guid confirmingUserId, IUserService userService);
        Task SaveUserAsync(OrganizationUser user, Guid? savingUserId, IEnumerable<SelectionReadOnly> collections);
        Task DeleteUserAsync(Guid organizationId, Guid organizationUserId, Guid? deletingUserId);
        Task DeleteUserAsync(Guid organizationId, Guid userId);
        Task<List<Tuple<OrganizationUser, string>>> DeleteUsersAsync(Guid organizationId,
            IEnumerable<Guid> organizationUserIds, Guid? deletingUserId);
        Task UpdateUserGroupsAsync(OrganizationUser organizationUser, IEnumerable<Guid> groupIds, Guid? loggedInUserId);
        Task UpdateUserResetPasswordEnrollmentAsync(Guid organizationId, Guid userId, string resetPasswordKey, Guid? callingUserId);
        Task<OrganizationLicense> GenerateLicenseAsync(Guid organizationId, Guid installationId);
        Task<OrganizationLicense> GenerateLicenseAsync(Organization organization, Guid installationId,
            int? version = null);
        Task ImportAsync(Guid organizationId, Guid? importingUserId, IEnumerable<ImportedGroup> groups,
            IEnumerable<ImportedOrganizationUser> newUsers, IEnumerable<string> removeUserExternalIds,
            bool overwriteExisting);
        Task DeleteSsoUserAsync(Guid userId, Guid? organizationId);
        Task<Organization> UpdateOrganizationKeysAsync(Guid orgId, string publicKey, string privateKey);
        Task<bool> HasConfirmedOwnersExceptAsync(Guid organizationId, IEnumerable<Guid> organizationUsersId, bool includeProvider = true);
        Task DeactivateUserAsync(OrganizationUser organizationUser, Guid? disablingUserId);
        Task<List<Tuple<OrganizationUser, string>>> DeactivateUsersAsync(Guid organizationId,
            IEnumerable<Guid> organizationUserIds, Guid? disablingUserId);
        Task ActivateUserAsync(OrganizationUser organizationUser, Guid? enablingUserId);
        Task<List<Tuple<OrganizationUser, string>>> ActivateUsersAsync(Guid organizationId,
            IEnumerable<Guid> organizationUserIds, Guid? enablingUserId);
    }
}
