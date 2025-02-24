using System;
using System.Collections.Generic;
namespace TaskManagementApi.Models;
public partial class TaskComment
{
    public int Id { get; set; }

    public int TaskId { get; set; }

    public int UserId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual Task? Task { get; set; } = null!;

    public virtual User? User { get; set; } = null!;
}
