namespace microservicios.Core.Entities
{
    public class AutorEntity: Document
    {
        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string GradoAcademico { get; set; }
    }
}
