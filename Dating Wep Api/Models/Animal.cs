using Dating_Wep_Api.IRepo;

namespace Dating_Wep_Api.Models
{
    public class Animal : IAnimal
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Animal> GetAllAnimal()
        {
            List<Animal> animals = new List<Animal>();
            Animal a1 = new Animal();
            a1.Id = 1;
            a1.Name = "Tiger";
            animals.Add(a1);

            return animals;
            
        }
    }
}
