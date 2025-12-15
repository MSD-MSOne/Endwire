using System;
using System.Collections.Generic;
using EndWire.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Task = EndWire.Domain.Models.Task;

namespace EndWire.Infrastructure;

public partial class EndWireContext : DbContext
{
    public EndWireContext()
    {
    }

    public EndWireContext(DbContextOptions<EndWireContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Apilist> Apilists { get; set; }

    public virtual DbSet<Apiresponse> Apiresponses { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientTask> ClientTasks { get; set; }

    public virtual DbSet<ClientUser> ClientUsers { get; set; }

    public virtual DbSet<FailedLogin> FailedLogins { get; set; }

    public virtual DbSet<FavoriteContact> FavoriteContacts { get; set; }

    public virtual DbSet<Feature> Features { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Industry> Industries { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Nudge> Nudges { get; set; }

    public virtual DbSet<NudgeResource> NudgeResources { get; set; }

    public virtual DbSet<NudgeType> NudgeTypes { get; set; }

    public virtual DbSet<RegistrationToken> RegistrationTokens { get; set; }

    public virtual DbSet<Reminder> Reminders { get; set; }

    public virtual DbSet<Resource> Resources { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RoleFeature> RoleFeatures { get; set; }

    public virtual DbSet<RoleNudge> RoleNudges { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<SystemDefault> SystemDefaults { get; set; }

    public virtual DbSet<SystemMessage> SystemMessages { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserGroup> UserGroups { get; set; }

    public virtual DbSet<UserInbox> UserInboxes { get; set; }

    public virtual DbSet<UserLocation> UserLocations { get; set; }

    public virtual DbSet<UserOutbox> UserOutboxes { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<UserTask> UserTasks { get; set; }

    public virtual DbSet<UserType> UserTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseSqlServer("Server= 34.135.219.232;Database=EndWire_Dev;User Id=JeffLi;Password=l6hU_q[A,37619;TrustServerCertificate=True");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Apilist>(entity =>
        {
            entity.ToTable("APIList");

            entity.Property(e => e.ApilistId).HasColumnName("APIListID");
            entity.Property(e => e.ApiSp)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("API_SP");
            entity.Property(e => e.Apicode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("APICode");
            entity.Property(e => e.Apiname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("APIName");
        });

        modelBuilder.Entity<Apiresponse>(entity =>
        {
            entity.ToTable("APIResponses");

            entity.Property(e => e.ApiresponseId).HasColumnName("APIResponseID");
            entity.Property(e => e.Response).IsUnicode(false);
            entity.Property(e => e.ResponseCode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.StoredProcedure)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.ClientId).HasName("Clients_pk");

            entity.Property(e => e.ClientId).HasColumnName("ClientID");
            entity.Property(e => e.Client1)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Client");
            entity.Property(e => e.IndustryId).HasColumnName("IndustryID");
            entity.Property(e => e.IndustryIndustryId).HasColumnName("Industry_IndustryID");

            entity.HasOne(d => d.IndustryIndustry).WithMany(p => p.Clients)
                .HasForeignKey(d => d.IndustryIndustryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Clients_Industry");
        });

        modelBuilder.Entity<ClientTask>(entity =>
        {
            entity.HasKey(e => e.ClientTaskId).HasName("ClientTasks_pk");

            entity.Property(e => e.ClientTaskId)
                .ValueGeneratedNever()
                .HasColumnName("ClientTaskID");
            entity.Property(e => e.ClientId).HasColumnName("ClientID");
            entity.Property(e => e.ClientsClientId).HasColumnName("Clients_ClientID");
            entity.Property(e => e.TaskId).HasColumnName("TaskID");
            entity.Property(e => e.TasksTaskId).HasColumnName("Tasks_TaskID");

            entity.HasOne(d => d.ClientsClient).WithMany(p => p.ClientTasks)
                .HasForeignKey(d => d.ClientsClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ClientTasks_Clients");

            entity.HasOne(d => d.TasksTask).WithMany(p => p.ClientTasks)
                .HasForeignKey(d => d.TasksTaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ClientTasks_Tasks");
        });

        modelBuilder.Entity<ClientUser>(entity =>
        {
            entity.HasKey(e => e.ClientUserId).HasName("ClientUsers_pk");

            entity.Property(e => e.ClientUserId)
                .ValueGeneratedNever()
                .HasColumnName("ClientUserID");
            entity.Property(e => e.ClientId).HasColumnName("ClientID");
            entity.Property(e => e.ClientsClientId).HasColumnName("Clients_ClientID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.UsersUserId).HasColumnName("Users_UserID");

            entity.HasOne(d => d.ClientsClient).WithMany(p => p.ClientUsers)
                .HasForeignKey(d => d.ClientsClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ClientUsers_Clients");

            entity.HasOne(d => d.UsersUser).WithMany(p => p.ClientUsers)
                .HasForeignKey(d => d.UsersUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ClientUsers_Users");
        });

        modelBuilder.Entity<FailedLogin>(entity =>
        {
            entity.HasKey(e => e.FailedLoginId).HasName("FailedLogins_pk");

            entity.Property(e => e.FailedLoginId).HasColumnName("FailedLoginID");
            entity.Property(e => e.DateAdded)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.UserName)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UserPassword)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<FavoriteContact>(entity =>
        {
            entity.HasKey(e => e.FavoriteContactId).HasName("FavoriteContacts_pk");

            entity.Property(e => e.FavoriteContactId)
                .ValueGeneratedNever()
                .HasColumnName("FavoriteContactID");
            entity.Property(e => e.DateAdded).HasColumnType("datetime");
            entity.Property(e => e.FavoriteContactUserId).HasColumnName("FavoriteContactUserID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.UsersUserId).HasColumnName("Users_UserID");

            entity.HasOne(d => d.UsersUser).WithMany(p => p.FavoriteContacts)
                .HasForeignKey(d => d.UsersUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FavoriteContacts_Users");
        });

        modelBuilder.Entity<Feature>(entity =>
        {
            entity.HasKey(e => e.FeatureId).HasName("Features_pk");

            entity.Property(e => e.FeatureId).HasColumnName("FeatureID");
            entity.Property(e => e.Feature1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Feature");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.GroupId).HasName("Groups_pk");

            entity.Property(e => e.GroupId)
                .ValueGeneratedNever()
                .HasColumnName("GroupID");
            entity.Property(e => e.Group1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Group");
        });

        modelBuilder.Entity<Industry>(entity =>
        {
            entity.HasKey(e => e.IndustryId).HasName("Industry_pk");

            entity.ToTable("Industry");

            entity.Property(e => e.IndustryId).HasColumnName("IndustryID");
            entity.Property(e => e.Industry1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Industry");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.LocationId).HasName("Locations_pk");

            entity.Property(e => e.LocationId).HasColumnName("LocationID");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ClientId).HasColumnName("ClientID");
            entity.Property(e => e.ClientsClientId).HasColumnName("Clients_ClientID");
            entity.Property(e => e.CountryCode)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Location1)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("Location");
            entity.Property(e => e.State)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Street1)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Street2)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ZipCode)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.ClientsClient).WithMany(p => p.Locations)
                .HasForeignKey(d => d.ClientsClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Locations_Clients");
        });

        modelBuilder.Entity<Nudge>(entity =>
        {
            entity.HasKey(e => e.NudgeId).HasName("Nudges_pk");

            entity.Property(e => e.NudgeId)
                .ValueGeneratedNever()
                .HasColumnName("NudgeID");
            entity.Property(e => e.Nudge1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Nudge");
            entity.Property(e => e.NudgeTypeId).HasColumnName("NudgeTypeID");
        });

        modelBuilder.Entity<NudgeResource>(entity =>
        {
            entity.HasKey(e => e.NudgeResourceId).HasName("NudgeResource_pk");

            entity.ToTable("NudgeResource");

            entity.Property(e => e.NudgeResourceId)
                .ValueGeneratedNever()
                .HasColumnName("NudgeResourceID");
            entity.Property(e => e.NudgeId).HasColumnName("NudgeID");
            entity.Property(e => e.NudgesNudgeId).HasColumnName("Nudges_NudgeID");
            entity.Property(e => e.ResourceId).HasColumnName("ResourceID");
            entity.Property(e => e.ResourcesResourceId).HasColumnName("Resources_ResourceID");

            entity.HasOne(d => d.NudgesNudge).WithMany(p => p.NudgeResources)
                .HasForeignKey(d => d.NudgesNudgeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("NudgeResource_Nudges");

            entity.HasOne(d => d.ResourcesResource).WithMany(p => p.NudgeResources)
                .HasForeignKey(d => d.ResourcesResourceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("NudgeResource_Resources");
        });

        modelBuilder.Entity<NudgeType>(entity =>
        {
            entity.HasKey(e => e.NudgeTypeId).HasName("NudgeTypes_pk");

            entity.Property(e => e.NudgeTypeId)
                .ValueGeneratedNever()
                .HasColumnName("NudgeTypeID");
            entity.Property(e => e.NudgeType1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NudgeType");
            entity.Property(e => e.NudgesNudgeId).HasColumnName("Nudges_NudgeID");

            entity.HasOne(d => d.NudgesNudge).WithMany(p => p.NudgeTypes)
                .HasForeignKey(d => d.NudgesNudgeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("NudgeTypes_Nudges");
        });

        modelBuilder.Entity<RegistrationToken>(entity =>
        {
            entity.HasKey(e => e.RegistrationTokenId).HasName("RegistrationTokens_pk");

            entity.Property(e => e.RegistrationTokenId)
                .ValueGeneratedNever()
                .HasColumnName("RegistrationTokenID");
            entity.Property(e => e.DateAdded).HasColumnType("datetime");
            entity.Property(e => e.RegistrationToken1)
                .HasMaxLength(50)
                .HasColumnName("RegistrationToken");
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<Reminder>(entity =>
        {
            entity.HasKey(e => e.ReminderId).HasName("Reminders_pk");

            entity.Property(e => e.ReminderId)
                .ValueGeneratedNever()
                .HasColumnName("ReminderID");
            entity.Property(e => e.DateAdded).HasColumnType("datetime");
            entity.Property(e => e.Reminder1)
                .HasColumnType("text")
                .HasColumnName("Reminder");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.UsersUserId).HasColumnName("Users_UserID");

            entity.HasOne(d => d.UsersUser).WithMany(p => p.Reminders)
                .HasForeignKey(d => d.UsersUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Reminders_Users");
        });

        modelBuilder.Entity<Resource>(entity =>
        {
            entity.HasKey(e => e.ResourceId).HasName("Resources_pk");

            entity.Property(e => e.ResourceId)
                .ValueGeneratedNever()
                .HasColumnName("ResourceID");
            entity.Property(e => e.LocationId).HasColumnName("LocationID");
            entity.Property(e => e.LocationsLocationId).HasColumnName("Locations_LocationID");
            entity.Property(e => e.ResourceName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.LocationsLocation).WithMany(p => p.Resources)
                .HasForeignKey(d => d.LocationsLocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Spaces_Locations");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("Roles_pk");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Role1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Role");
        });

        modelBuilder.Entity<RoleFeature>(entity =>
        {
            entity.HasKey(e => e.RoleFeatureId).HasName("RoleFeatures_pk");

            entity.Property(e => e.RoleFeatureId).HasColumnName("RoleFeatureID");
            entity.Property(e => e.FeatureId).HasColumnName("FeatureID");
            entity.Property(e => e.FeaturesFeatureId).HasColumnName("Features_FeatureID");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RolesRoleId).HasColumnName("Roles_RoleID");

            entity.HasOne(d => d.FeaturesFeature).WithMany(p => p.RoleFeatures)
                .HasForeignKey(d => d.FeaturesFeatureId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RoleFeatures_Features");

            entity.HasOne(d => d.RolesRole).WithMany(p => p.RoleFeatures)
                .HasForeignKey(d => d.RolesRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RoleFeatures_Roles");
        });

        modelBuilder.Entity<RoleNudge>(entity =>
        {
            entity.HasKey(e => e.RoleNudgeId).HasName("RoleNudges_pk");

            entity.Property(e => e.RoleNudgeId)
                .ValueGeneratedNever()
                .HasColumnName("RoleNudgeID");
            entity.Property(e => e.NudgeId).HasColumnName("NudgeID");
            entity.Property(e => e.NudgesNudgeId).HasColumnName("Nudges_NudgeID");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RolesRoleId).HasColumnName("Roles_RoleID");

            entity.HasOne(d => d.NudgesNudge).WithMany(p => p.RoleNudges)
                .HasForeignKey(d => d.NudgesNudgeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RoleNudges_Nudges");

            entity.HasOne(d => d.RolesRole).WithMany(p => p.RoleNudges)
                .HasForeignKey(d => d.RolesRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RoleNudges_Roles");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("Sessions_pk");

            entity.Property(e => e.SessionId).HasColumnName("SessionID");
            entity.Property(e => e.AuthToken)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DateAdded)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DateEnded).HasColumnType("datetime");
            entity.Property(e => e.LocationId).HasColumnName("LocationID");
            entity.Property(e => e.LocationsLocationId).HasColumnName("Locations_LocationID");
            entity.Property(e => e.RegToken)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.LocationsLocation).WithMany(p => p.Sessions)
                .HasForeignKey(d => d.LocationsLocationId)
                .HasConstraintName("Sessions_Locations");
        });

        modelBuilder.Entity<SystemDefault>(entity =>
        {
            entity.Property(e => e.SystemDefaultId).HasColumnName("SystemDefaultID");
        });

        modelBuilder.Entity<SystemMessage>(entity =>
        {
            entity.HasKey(e => e.SystemMessageId).HasName("SystemMessages_pk");

            entity.Property(e => e.SystemMessageId)
                .ValueGeneratedNever()
                .HasColumnName("SystemMessageID");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.TaskId).HasName("Tasks_pk");

            entity.Property(e => e.TaskId)
                .ValueGeneratedNever()
                .HasColumnName("TaskID");
            entity.Property(e => e.Task1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Task");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("Users_pk");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.ClientId).HasColumnName("ClientID");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Passcode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Picture)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("UserImageURL");
            entity.Property(e => e.UserName)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UserPassword)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");
            entity.Property(e => e.UserTypesUserTypeId).HasColumnName("UserTypes_UserTypeID");

            entity.HasOne(d => d.UserTypesUserType).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserTypesUserTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Users_UserTypes");
        });

        modelBuilder.Entity<UserGroup>(entity =>
        {
            entity.HasKey(e => e.UserGroupId).HasName("UserGroups_pk");

            entity.Property(e => e.UserGroupId)
                .ValueGeneratedNever()
                .HasColumnName("UserGroupID");
            entity.Property(e => e.GroupId).HasColumnName("GroupID");
            entity.Property(e => e.GroupsGroupId).HasColumnName("Groups_GroupID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.UsersUserId).HasColumnName("Users_UserID");

            entity.HasOne(d => d.GroupsGroup).WithMany(p => p.UserGroups)
                .HasForeignKey(d => d.GroupsGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserGroups_Groups");

            entity.HasOne(d => d.UsersUser).WithMany(p => p.UserGroups)
                .HasForeignKey(d => d.UsersUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserGroups_Users");
        });

        modelBuilder.Entity<UserInbox>(entity =>
        {
            entity.HasKey(e => e.UserInboxId).HasName("UserInbox_pk");

            entity.ToTable("UserInbox");

            entity.Property(e => e.UserInboxId)
                .ValueGeneratedNever()
                .HasColumnName("UserInboxID");
            entity.Property(e => e.DateAdded).HasColumnType("datetime");
            entity.Property(e => e.DateRemoved).HasColumnType("datetime");
            entity.Property(e => e.NudgeId).HasColumnName("NudgeID");
            entity.Property(e => e.NudgesNudgeId).HasColumnName("Nudges_NudgeID");
            entity.Property(e => e.ReminderId).HasColumnName("ReminderID");
            entity.Property(e => e.RemindersReminderId).HasColumnName("Reminders_ReminderID");
            entity.Property(e => e.ResourceId).HasColumnName("ResourceID");
            entity.Property(e => e.TaskId).HasColumnName("TaskID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.UsersUserId).HasColumnName("Users_UserID");

            entity.HasOne(d => d.NudgesNudge).WithMany(p => p.UserInboxes)
                .HasForeignKey(d => d.NudgesNudgeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserInbox_Nudges");

            entity.HasOne(d => d.RemindersReminder).WithMany(p => p.UserInboxes)
                .HasForeignKey(d => d.RemindersReminderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserInbox_Reminders");

            entity.HasOne(d => d.UsersUser).WithMany(p => p.UserInboxes)
                .HasForeignKey(d => d.UsersUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserInbox_Users");
        });

        modelBuilder.Entity<UserLocation>(entity =>
        {
            entity.HasKey(e => new { e.UserLocationId, e.LocationsLocationId, e.UsersUserId }).HasName("UserLocations_pk");

            entity.Property(e => e.UserLocationId)
                .ValueGeneratedOnAdd()
                .HasColumnName("UserLocationID");
            entity.Property(e => e.LocationsLocationId).HasColumnName("Locations_LocationID");
            entity.Property(e => e.UsersUserId).HasColumnName("Users_UserID");
            entity.Property(e => e.LocationId).HasColumnName("LocationID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.LocationsLocation).WithMany(p => p.UserLocations)
                .HasForeignKey(d => d.LocationsLocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserLocation_Locations");

            entity.HasOne(d => d.UsersUser).WithMany(p => p.UserLocations)
                .HasForeignKey(d => d.UsersUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserLocation_Users");
        });

        modelBuilder.Entity<UserOutbox>(entity =>
        {
            entity.HasKey(e => e.UserOutboxId).HasName("UserOutbox_pk");

            entity.ToTable("UserOutbox");

            entity.Property(e => e.UserOutboxId)
                .ValueGeneratedNever()
                .HasColumnName("UserOutboxID");
            entity.Property(e => e.DateAdded).HasColumnType("datetime");
            entity.Property(e => e.DateRemoved).HasColumnType("datetime");
            entity.Property(e => e.NudgeId).HasColumnName("NudgeID");
            entity.Property(e => e.NudgesNudgeId).HasColumnName("Nudges_NudgeID");
            entity.Property(e => e.ReminderId).HasColumnName("ReminderID");
            entity.Property(e => e.RemindersReminderId).HasColumnName("Reminders_ReminderID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.UsersUserId).HasColumnName("Users_UserID");

            entity.HasOne(d => d.NudgesNudge).WithMany(p => p.UserOutboxes)
                .HasForeignKey(d => d.NudgesNudgeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserOutbox_Nudges");

            entity.HasOne(d => d.RemindersReminder).WithMany(p => p.UserOutboxes)
                .HasForeignKey(d => d.RemindersReminderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserOutbox_Reminders");

            entity.HasOne(d => d.UsersUser).WithMany(p => p.UserOutboxes)
                .HasForeignKey(d => d.UsersUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserOutbox_Users");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.UserRoleId).HasName("UserRoles_pk");

            entity.Property(e => e.UserRoleId).HasColumnName("UserRoleID");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RolesRoleId).HasColumnName("Roles_RoleID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.UsersUserId).HasColumnName("Users_UserID");

            entity.HasOne(d => d.RolesRole).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RolesRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserRoles_Roles");

            entity.HasOne(d => d.UsersUser).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UsersUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserRoles_Users");
        });

        modelBuilder.Entity<UserTask>(entity =>
        {
            entity.HasKey(e => e.UserTaskId).HasName("UserTasks_pk");

            entity.Property(e => e.UserTaskId)
                .ValueGeneratedNever()
                .HasColumnName("UserTaskID");
            entity.Property(e => e.TaskId).HasColumnName("TaskID");
            entity.Property(e => e.TaskStatusId).HasColumnName("TaskStatusID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<UserType>(entity =>
        {
            entity.HasKey(e => e.UserTypeId).HasName("UserTypes_pk");

            entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");
            entity.Property(e => e.UserType1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UserType");
        });
        modelBuilder.HasSequence("Clients_seq");
        modelBuilder.HasSequence("ClientTasks_seq");
        modelBuilder.HasSequence("ClientUsers_seq");
        modelBuilder.HasSequence("FailedLogins_seq");
        modelBuilder.HasSequence("FavoriteContacts_seq");
        modelBuilder.HasSequence("Features_seq");
        modelBuilder.HasSequence("Groups_seq");
        modelBuilder.HasSequence("Industry_seq");
        modelBuilder.HasSequence("Locations_seq");
        modelBuilder.HasSequence("NudgeResource_seq");
        modelBuilder.HasSequence("Nudges_seq");
        modelBuilder.HasSequence("NudgeTypes_seq");
        modelBuilder.HasSequence("RegistrationTokens_seq");
        modelBuilder.HasSequence("Reminders_seq");
        modelBuilder.HasSequence("Resources_seq");
        modelBuilder.HasSequence("RoleFeatures_seq");
        modelBuilder.HasSequence("RoleNudges_seq");
        modelBuilder.HasSequence("Roles_seq");
        modelBuilder.HasSequence("Sessions_seq");
        modelBuilder.HasSequence("SystemMessages_seq");
        modelBuilder.HasSequence("Tasks_seq");
        modelBuilder.HasSequence("UserGroups_seq");
        modelBuilder.HasSequence("UserInbox_seq");
        modelBuilder.HasSequence("UserOutbox_seq");
        modelBuilder.HasSequence("UserRoles_seq");
        modelBuilder.HasSequence("Users_seq");
        modelBuilder.HasSequence("UserTasks_seq");
        modelBuilder.HasSequence("UserTypes_seq");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
