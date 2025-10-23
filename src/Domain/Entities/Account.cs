using System;
using System.Collections.Generic;

namespace WorkManagement.Domain.Entities;

public class Account
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public string? Description { get; set; }
    public AccountType Type { get; set; } = AccountType.Customer;
    public string? Status { get; set; }
    public ICollection<Project> Projects { get; set; } = new List<Project>();
    public ICollection<AccountTag> Tags { get; set; } = new List<AccountTag>();
    public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }
}

public enum AccountType
{
    Customer,
    Supplier
}

public class AccountTag
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Value { get; set; } = string.Empty;
    public Guid AccountId { get; set; }
    public Account? Account { get; set; }
}

public class Contact
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FullName { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public Guid AccountId { get; set; }
    public Account? Account { get; set; }
}
