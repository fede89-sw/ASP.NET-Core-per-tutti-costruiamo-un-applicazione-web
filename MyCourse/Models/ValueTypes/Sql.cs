// classe creata solo per impedire di rendere i parametri 'orderby' e 'direction' nella query SQL in
// AdoNetCourseService dei SQLParameters, in quanto per noi non sono paramentri da passare come gli altri,
// ma parte integrante della query, diciamo che fa parte del testo che comporrà ad es.
// 'ORDER BY Title DESC'; non sono quindi valori ma istruzioni SQL

namespace MyCourse.Models.ValueTypes
{
    //Questa classe serve unicamente per indicare al servizio infrastrutturale SqliteAccessor
    //che un dato parametro non deve essere convertito in SqliteParameter
    public class Sql 
    {
        private Sql(string value)
        {
            Value = value;
        }
        //Proprietà per conservare il valore originale
        public string Value { get; }

        //Conversione da/per il tipo string
        public static explicit operator Sql(string value) => new Sql(value);
        public override string ToString() {
            return this.Value;
        }
    }
}