using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TaskManagementApi.Models;

public partial class Label
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    public virtual ICollection<TaskLabel> TaskLabels { get; set; } = new List<TaskLabel>();

    //public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
