using System.Data;
using System.Data.SqlClient;

// 

string connectionString = "server=localhost;database=MovieDB;" + "user id=sa;password=Mela14ad;TrustServerCertificate=True";
SqlConnection _sqlConnection = new SqlConnection(connectionString);

try
{
	_sqlConnection.Open();
	Console.WriteLine("Velkommen til IMDB Console App!");

	while (true)
	{
		Console.WriteLine("\nVælg en handling:");
		Console.WriteLine("1. Søg efter film");
		Console.WriteLine("2. Søg efter person");
		Console.WriteLine("3. Tilføj film");
		Console.WriteLine("4. Tilføj person");
		Console.WriteLine("5. Opdater filmoplysninger");
		Console.WriteLine("6. Slet film");
		Console.WriteLine("0. Afslut");

		Console.Write("\nIndtast dit valg (0-6):");
		string? choice = Console.ReadLine();

		switch (choice)
		{
			case "1":
				SearchMovie(_sqlConnection);
				break;

			case "2":
				SearchPerson();
				break;

			case "3":
				AddMovie();
				break;

			case "4":
				AddPerson();
				break;

			case "5":
				UpdateMovie();
				break;

			case "6":
				DeleteMovie();
				break;

			case "0":
				_sqlConnection.Close();
				return; // Afslut programmet

			default:
				Console.WriteLine("\nUgyldigt valg. Prøv igen.");
				break;
		}
	}
}

catch (Exception ex)
{
	Console.WriteLine("\nDer opstod en fejl ved oprettelse af forbindelsen: " + ex.Message);
}
finally
{
	_sqlConnection.Close();
}


static void SearchMovie(SqlConnection sqlConnection)
{
	Console.Write("Indtast søgeterm: ");
	string? searchTerm = Console.ReadLine();

	SqlCommand sqlCommand = new SqlCommand("SELECT * FROM dbo.fn_SearchMovieTitles(@searchTerm)", sqlConnection);
	sqlCommand.Parameters.AddWithValue("@searchTerm", searchTerm);

	try
	{
		SqlDataReader reader = sqlCommand.ExecuteReader();
		while (reader.Read())
		{
			Console.WriteLine($"tconst: {reader["tconst"]}, primaryTitle: {reader["primaryTitle"]}");
		}
		reader.Close();
	}
	catch (Exception ex)
	{
		Console.WriteLine("An error occurred: " + ex.Message);
	}
}




void SearchPerson()
{
	Console.Write("Indtast søgeterm: ");
	string searchTerm = Console.ReadLine();

	using (SqlCommand cmd = new SqlCommand("sp_SearchPersons", _sqlConnection))
	{
		cmd.CommandType = CommandType.StoredProcedure;
		cmd.Parameters.AddWithValue("@searchTerm", searchTerm);

		using (SqlDataReader reader = cmd.ExecuteReader())
		{
			while (reader.Read())
			{
				Console.WriteLine($"nconst: {reader["nconst"]}, primaryName: {reader["primaryName"]}");
			}
		}
	}
}


void AddMovie()
{
	try
	{
		Console.Write("Indtast titleType: ");
		string titleType = Console.ReadLine();

		Console.Write("Indtast primaryTitle: ");
		string primaryTitle = Console.ReadLine();

		Console.Write("Indtast originalTitle: ");
		string originalTitle = Console.ReadLine();

		Console.Write("Indtast isAdult (0 for Nej, 1 for Ja): ");
		int isAdult = int.Parse(Console.ReadLine());

		Console.Write("Indtast startYear: ");
		int startYear = int.Parse(Console.ReadLine());

		Console.Write("Indtast endYear: ");
		int endYear = int.Parse(Console.ReadLine());

		Console.Write("Indtast runtimeMinutes: ");
		int runtimeMinutes = int.Parse(Console.ReadLine());

		using (SqlCommand cmd = new SqlCommand("sp_AddMovie", _sqlConnection))
		{
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@titleType", titleType);
			cmd.Parameters.AddWithValue("@primaryTitle", primaryTitle);
			cmd.Parameters.AddWithValue("@originalTitle", originalTitle);
			cmd.Parameters.AddWithValue("@isAdult", isAdult);
			cmd.Parameters.AddWithValue("@startYear", startYear);
			cmd.Parameters.AddWithValue("@endYear", endYear);
			cmd.Parameters.AddWithValue("@runtimeMinutes", runtimeMinutes);

			cmd.ExecuteNonQuery();
			Console.WriteLine("Film tilføjet med succes.");
		}
	}
	catch (Exception ex)
	{
		Console.WriteLine("An error occurred: " + ex.Message);
	}
}

void AddPerson()
{
	try
	{
		Console.Write("Indtast primaryName: ");
		string primaryName = Console.ReadLine();

		Console.Write("Indtast birthYear: ");
		int birthYear = int.Parse(Console.ReadLine());

		Console.Write("Indtast deathYear: ");
		int deathYear = int.Parse(Console.ReadLine());

		using (SqlCommand cmd = new SqlCommand("sp_AddName", _sqlConnection))
		{
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@primaryName", primaryName);
			cmd.Parameters.AddWithValue("@birthYear", birthYear);
			cmd.Parameters.AddWithValue("@deathYear", deathYear);

			cmd.ExecuteNonQuery();
			Console.WriteLine("Navn tilføjet med succes.");
		}
	}
	catch (Exception ex)
	{
		Console.WriteLine("Der opstod en fejl: " + ex.Message);
	}
}


void UpdateMovie()
{
	try
	{
		Console.Write("Indtast tconst: ");
		string tconst = Console.ReadLine();

		Console.Write("Indtast ny titleType (tryk Enter for at beholde nuværende værdi): ");
		string titleTypeInput = Console.ReadLine();
		string titleType = string.IsNullOrWhiteSpace(titleTypeInput) ? null : titleTypeInput;

		Console.Write("Indtast ny primaryTitle (tryk Enter for at beholde nuværende værdi): ");
		string primaryTitleInput = Console.ReadLine();
		string primaryTitle = string.IsNullOrWhiteSpace(primaryTitleInput) ? null : primaryTitleInput;

		Console.Write("Indtast ny originalTitle (tryk Enter for at beholde nuværende værdi): ");
		string originalTitleInput = Console.ReadLine();
		string originalTitle = string.IsNullOrWhiteSpace(originalTitleInput) ? null : originalTitleInput;

		Console.Write("Indtast isAdult (0 for Nej, 1 for Ja, tryk Enter for at beholde nuværende værdi): ");
		int? isAdult = null;
		string isAdultInput = Console.ReadLine();
		if (!string.IsNullOrWhiteSpace(isAdultInput))
		{
			isAdult = int.Parse(isAdultInput);
		}

		Console.Write("Indtast ny startYear (tryk Enter for at beholde nuværende værdi): ");
		int? startYear = null;
		string startYearInput = Console.ReadLine();
		if (!string.IsNullOrWhiteSpace(startYearInput))
		{
			startYear = int.Parse(startYearInput);
		}

		Console.Write("Indtast ny endYear (tryk Enter for at beholde nuværende værdi): ");
		int? endYear = null;
		string endYearInput = Console.ReadLine();
		if (!string.IsNullOrWhiteSpace(endYearInput))
		{
			endYear = int.Parse(endYearInput);
		}

		Console.Write("Indtast ny runtimeMinutes (tryk Enter for at beholde nuværende værdi): ");
		int? runtimeMinutes = null;
		string runtimeMinutesInput = Console.ReadLine();
		if (!string.IsNullOrWhiteSpace(runtimeMinutesInput))
		{
			runtimeMinutes = int.Parse(runtimeMinutesInput);
		}

		using (SqlCommand cmd = new SqlCommand("sp_UpdateMovie", _sqlConnection))
		{
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@tconst", tconst);
			cmd.Parameters.AddWithValue("@titleType", titleType);
			cmd.Parameters.AddWithValue("@primaryTitle", primaryTitle);
			cmd.Parameters.AddWithValue("@originalTitle", originalTitle);
			cmd.Parameters.AddWithValue("@isAdult", isAdult);
			cmd.Parameters.AddWithValue("@startYear", startYear);
			cmd.Parameters.AddWithValue("@endYear", endYear);
			cmd.Parameters.AddWithValue("@runtimeMinutes", runtimeMinutes);

			cmd.ExecuteNonQuery();
			Console.WriteLine("Film opdateret med succes.");
		}
	}
	catch (Exception ex)
	{
		Console.WriteLine("Der opstod en fejl: " + ex.Message);
	}
}


void DeleteMovie()
{
	Console.Write("Indtast tconst for den film, du vil slette: ");
	string? tconst = Console.ReadLine();

	using (SqlCommand cmd = new SqlCommand("sp_DeleteMovie", _sqlConnection))
	{
		cmd.CommandType = CommandType.StoredProcedure;
		cmd.Parameters.AddWithValue("@tconst", tconst);

		cmd.ExecuteNonQuery();
		Console.WriteLine("Film slettet med succes.");
	}
}
