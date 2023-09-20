using System;
using System.ComponentModel.DataAnnotations;

public class DateNotInPastAttribute : ValidationAttribute
{
    // Este método se llama durante la validación y determina si la fecha en la propiedad es válida.
    public override bool IsValid(object value)
    {
        // Convierte el valor recibido en un objeto DateTime.
        DateTime date = (DateTime)value;

        // Compara la fecha con la fecha actual (sin incluir la hora) para verificar si no está en el pasado.
        // Si la fecha es mayor o igual a la fecha actual, se considera válida.
        return date.Date >= DateTime.Now.Date;
    }
}
