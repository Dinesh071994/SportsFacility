using System;

namespace SportsFacility.Entity.Entities
{
    public class UserMembershipDependent : BaseEntity
    {
        public Guid UserMembershipId { get; set; }
        public UserMembership UserMembership { get; set; } = null!;

        public Guid DependentUserId { get; set; }
        public User DependentUser { get; set; } = null!;
    }
}
