using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TaskManagementApi.Models;

[Index(nameof(Name), IsUnique = true)]
public partial class Category
{
    public int Id { get; set; }
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }
    [JsonPropertyName("tasks")]
    public virtual List<Task> Tasks { get; set; } = new List<Task>();
    //public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
