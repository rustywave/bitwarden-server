﻿using Bit.Core.Context;
using Bit.Core.Entities;
using Bit.Core.Enums;
using Bit.Core.Models;
using Bit.Core.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Bit.Core.Services
{
    public class NotificationsApiPushNotificationService : BaseIdentityClientService, IPushNotificationService
    {
        private readonly GlobalSettings _globalSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NotificationsApiPushNotificationService(
            IHttpClientFactory httpFactory,
            GlobalSettings globalSettings,
            IHttpContextAccessor httpContextAccessor,
            ILogger<NotificationsApiPushNotificationService> logger)
            : base(
                httpFactory,
                globalSettings.BaseServiceUri.InternalNotifications,
                globalSettings.BaseServiceUri.InternalIdentity,
                "internal",
                $"internal.{globalSettings.ProjectName}",
                globalSettings.InternalIdentityKey,
                logger)
        {
            _globalSettings = globalSettings;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task PushSyncCipherCreateAsync(Cipher cipher, IEnumerable<Guid> collectionIds)
        {
            await PushCipherAsync(cipher, PushType.SyncCipherCreate, collectionIds);
        }

        public async Task PushSyncCipherUpdateAsync(Cipher cipher, IEnumerable<Guid> collectionIds)
        {
            await PushCipherAsync(cipher, PushType.SyncCipherUpdate, collectionIds);
        }

        public async Task PushSyncCipherDeleteAsync(Cipher cipher)
        {
            await PushCipherAsync(cipher, PushType.SyncLoginDelete, null);
        }

        private async Task PushCipherAsync(Cipher cipher, PushType type, IEnumerable<Guid> collectionIds)
        {
            if (cipher.OrganizationId.HasValue)
            {
                var message = new SyncCipherPushNotification
                {
                    Id = cipher.Id,
                    OrganizationId = cipher.OrganizationId,
                    RevisionDate = cipher.RevisionDate,
                    CollectionIds = collectionIds,
                };

                await SendMessageAsync(type, message, true);
            }
            else if (cipher.UserId.HasValue)
            {
                var message = new SyncCipherPushNotification
                {
                    Id = cipher.Id,
                    UserId = cipher.UserId,
                    RevisionDate = cipher.RevisionDate,
                    CollectionIds = collectionIds,
                };

                await SendMessageAsync(type, message, true);
            }
        }

        public async Task PushSyncFolderCreateAsync(Folder folder)
        {
            await PushFolderAsync(folder, PushType.SyncFolderCreate);
        }

        public async Task PushSyncFolderUpdateAsync(Folder folder)
        {
            await PushFolderAsync(folder, PushType.SyncFolderUpdate);
        }

        public async Task PushSyncFolderDeleteAsync(Folder folder)
        {
            await PushFolderAsync(folder, PushType.SyncFolderDelete);
        }

        private async Task PushFolderAsync(Folder folder, PushType type)
        {
            var message = new SyncFolderPushNotification
            {
                Id = folder.Id,
                UserId = folder.UserId,
                RevisionDate = folder.RevisionDate
            };

            await SendMessageAsync(type, message, true);
        }

        public async Task PushSyncCiphersAsync(Guid userId)
        {
            await PushUserAsync(userId, PushType.SyncCiphers);
        }

        public async Task PushSyncVaultAsync(Guid userId)
        {
            await PushUserAsync(userId, PushType.SyncVault);
        }

        public async Task PushSyncOrgKeysAsync(Guid userId)
        {
            await PushUserAsync(userId, PushType.SyncOrgKeys);
        }

        public async Task PushSyncSettingsAsync(Guid userId)
        {
            await PushUserAsync(userId, PushType.SyncSettings);
        }

        public async Task PushLogOutAsync(Guid userId)
        {
            await PushUserAsync(userId, PushType.LogOut);
        }

        private async Task PushUserAsync(Guid userId, PushType type)
        {
            var message = new UserPushNotification
            {
                UserId = userId,
                Date = DateTime.UtcNow
            };

            await SendMessageAsync(type, message, false);
        }

        public async Task PushSyncSendCreateAsync(Send send)
        {
            await PushSendAsync(send, PushType.SyncSendCreate);
        }

        public async Task PushSyncSendUpdateAsync(Send send)
        {
            await PushSendAsync(send, PushType.SyncSendUpdate);
        }

        public async Task PushSyncSendDeleteAsync(Send send)
        {
            await PushSendAsync(send, PushType.SyncSendDelete);
        }

        private async Task PushSendAsync(Send send, PushType type)
        {
            if (send.UserId.HasValue)
            {
                var message = new SyncSendPushNotification
                {
                    Id = send.Id,
                    UserId = send.UserId.Value,
                    RevisionDate = send.RevisionDate
                };

                await SendMessageAsync(type, message, false);
            }
        }

        private async Task SendMessageAsync<T>(PushType type, T payload, bool excludeCurrentContext)
        {
            var contextId = GetContextIdentifier(excludeCurrentContext);
            var request = new PushNotificationData<T>(type, payload, contextId);
            await SendAsync(HttpMethod.Post, "send", request);
        }

        private string GetContextIdentifier(bool excludeCurrentContext)
        {
            if (!excludeCurrentContext)
            {
                return null;
            }

            var currentContext = _httpContextAccessor?.HttpContext?.
                RequestServices.GetService(typeof(ICurrentContext)) as ICurrentContext;
            return currentContext?.DeviceIdentifier;
        }

        public Task SendPayloadToUserAsync(string userId, PushType type, object payload, string identifier,
            string deviceId = null)
        {
            // Noop
            return Task.FromResult(0);
        }

        public Task SendPayloadToOrganizationAsync(string orgId, PushType type, object payload, string identifier,
            string deviceId = null)
        {
            // Noop
            return Task.FromResult(0);
        }
    }
}
