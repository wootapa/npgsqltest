using System;
using System.Linq;
using Npgsql;

try
{
    await using var conn = new NpgsqlConnection(args.First());
    await conn.OpenAsync();

    string sql = args.Skip(1).FirstOrDefault();

    if (sql != null)
    {
        var command = new NpgsqlCommand(sql, conn);
        var reader = command.ExecuteReader();

        // Print columns
        Console.WriteLine(string.Join(',', (await reader.GetColumnSchemaAsync()).Select(foo => foo.ColumnName)));

        // Print values
        while (await reader.ReadAsync())
        {
            Console.WriteLine(string.Join(',', Enumerable.Range(0, reader.FieldCount).Select(i => reader[i])));
        }
    }

    Console.WriteLine("Success");
    Environment.Exit(0);
}
catch (Exception e)
{
    Console.WriteLine(e.StackTrace);
    Environment.Exit(1);
}