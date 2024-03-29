﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P01_StudentSystem.Data.Models
{
    public class Homework
    {
        [Key]
        public int HomeworkId { get; set; }
        [Required]
        [Column(TypeName = "varchar(MAX)")]
        public string Content { get; set; }
        [Required]
        public ContentType ContentType { get; set; }
        [Required]
        public DateTime SubmissionTime { get; set; }
        [Required]
        public int StudentId { get; set; }
        public Student Student { get; set; }
        [Required]
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
