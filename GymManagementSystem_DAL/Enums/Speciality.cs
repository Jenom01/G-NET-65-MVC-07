using System.ComponentModel.DataAnnotations;

namespace GymManagementSystem_DAL.Enums
{
    public enum Speciality
    {
        Boxing = 1,
        Yoga = 2,
        CrossFit = 3,
        Pilates = 4,
        Zumba = 5,
        Cycling = 6,
        [Display(Name = "Strength Training")]
        StrengthTraining = 7,
        Cardio = 8,
        [Display(Name = "General Fitness")]
        GeneralFitness = 9
    }
}
