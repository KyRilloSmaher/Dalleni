using Dalleni.Domin.Enums;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.Models.Base;

namespace Dalleni.Domin.Models
{
public class OfficialEntityMembership : DomainEntity
{
    private OfficialEntityMembership()
    {
    }

    private OfficialEntityMembership(Guid officialEntityId,Guid userId,EntityRole role)
    {
        Id = Guid.NewGuid();

        OfficialEntityId = DomainGuard.AgainstEmpty(officialEntityId,nameof(officialEntityId));
        UserId = DomainGuard.AgainstEmpty(userId,nameof(userId));
        Role = role;
    }

    public Guid Id { get; private set; }

    public Guid OfficialEntityId { get; private set; }

    public OfficialEntity OfficialEntity { get; private set; } = null!;

    public Guid UserId { get; private set; }

    public ApplicationUser User { get; private set; } = null!;

    public EntityRole Role { get; private set; }

    public bool IsActive { get; private set; } = true;

    public static OfficialEntityMembership Create(Guid officialEntityId,Guid userId,EntityRole role)
    {
        return new OfficialEntityMembership(
            officialEntityId,
            userId,
            role);
    }

    public void ChangeRole(EntityRole role)
    {
        EnsureNotDeleted();
        Role = role;
        MarkUpdated();
    }

    public void Deactivate()
    {
        EnsureNotDeleted();
        IsActive = false;
        MarkUpdated();
    }

    public void Activate()
    {
        EnsureNotDeleted();
        IsActive = true;
        MarkUpdated();
    }
    public bool CanManageMembers()
        {
                return IsActive && Role switch
                                {
                                    EntityRole.Owner => true,
                                    EntityRole.Admin => true,
                                    _ => false
                                
                                    };
        }
    public bool CanPublishPosts()
        {
            return IsActive &&
                   Role is
                       EntityRole.Owner or
                       EntityRole.Admin or
                       EntityRole.Moderator;
        }
    public bool CanManageServices()
        {
          return IsActive &&
                   Role is
                       EntityRole.Owner or
                       EntityRole.Admin or
                       EntityRole.Moderator;
        }
    public bool CanAnswerOfficially()
        {
            return IsActive &&
                   Role is
                       EntityRole.Owner or
                       EntityRole.Admin or
                       EntityRole.Moderator or
                       EntityRole.Staff;
        }
}
}
