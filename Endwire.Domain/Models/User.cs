using System;
using System.Collections.Generic;

namespace EndWire.Domain.Models;

public partial class User
{
    public int UserId { get; set; }

    public int ClientId { get; set; }

    public int UserTypeId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string UserPassword { get; set; } = null!;

    public string Picture { get; set; } = null!;

    public string Passcode { get; set; } = null!;

    public int UserTypesUserTypeId { get; set; }

    public virtual ICollection<ClientUser> ClientUsers { get; } = new List<ClientUser>();

    public virtual ICollection<FavoriteContact> FavoriteContacts { get; } = new List<FavoriteContact>();

    public virtual ICollection<Reminder> Reminders { get; } = new List<Reminder>();

    public virtual ICollection<UserGroup> UserGroups { get; } = new List<UserGroup>();

    public virtual ICollection<UserInbox> UserInboxes { get; } = new List<UserInbox>();

    public virtual ICollection<UserLocation> UserLocations { get; } = new List<UserLocation>();

    public virtual ICollection<UserOutbox> UserOutboxes { get; } = new List<UserOutbox>();

    public virtual ICollection<UserRole> UserRoles { get; } = new List<UserRole>();

    public virtual UserType UserTypesUserType { get; set; } = null!;
}
