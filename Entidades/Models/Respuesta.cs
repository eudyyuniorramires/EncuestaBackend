namespace Entidades.Models { 

        public class Respuesta
        {
            public int Id { get; set; }
            public int PreguntaId { get; set; }
            public int? UsuarioId { get; set; }   // nullable si no siempre se usará
            public int? NumeroRespuesta { get; set; }

            public Pregunta Pregunta { get; set; }
         
            public string TextoRespuesta { get; set; }
       
        }

}
