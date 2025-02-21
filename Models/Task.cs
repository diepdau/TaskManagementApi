using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TaskManagementApi.Models;

public partial class Task
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public bool? IsCompleted { get; set; }

    public int UserId { get; set; }

    public int? CategoryId { get; set; }

    public DateTime? CreatedAt { get; set; }
    [JsonIgnore]
    public virtual Category? Category { get; set; }

    public virtual ICollection<TaskComment> TaskComments { get; set; } = new List<TaskComment>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Label> Labels { get; set; } = new List<Label>();
}
