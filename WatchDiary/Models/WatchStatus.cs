using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WatchDiary.Models
{
    public enum WatchStatus
    {
        watched, 
        watching, 
        planning, 
        paused, 
        dropped
    }
}
