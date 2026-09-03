


using Dalleni.Domin.Enums;

namespace Dalleni.Application.DTOs.Requests.OfficialEntities
{

    public class InviteEntityMemberRequestDto
    {
        
        public string Email { get; set; } = string.Empty;
        public EntityRole  Role {get;set;}   
    }
}