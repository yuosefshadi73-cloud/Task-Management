namespace Task_Management.Dtos.Task
{
    public class AddTaskDto
    {
        public string Title { get; set; }

        public string? Description { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public long StatusId { get; set; }
    }
}