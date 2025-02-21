﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskManagementApi.Models;

[Keyless]
public partial class TaskLabel
{
    public int? TaskId { get; set; }

    public int? LabelId { get; set; }

    [ForeignKey("LabelId")]
    public virtual Label? Label { get; set; }

    [ForeignKey("TaskId")]
    public virtual Task? Task { get; set; }
}
