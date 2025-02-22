﻿using Bit.Core.Entities;

namespace Bit.Core.OrganizationFeatures.OrganizationSponsorships.FamiliesForEnterprise.Interfaces
{
    public interface IRemoveSponsorshipCommand
    {
        Task RemoveSponsorshipAsync(OrganizationSponsorship sponsorship);
    }
}
