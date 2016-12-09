namespace Trady.Importer.Helper
{
    internal static class ImporterExtension
    {
        internal static bool IsRowNullOrWhiteSpace(this object[] row)
        {
            foreach (var r in row)
            {
                if (r == null || string.IsNullOrWhiteSpace(r.ToString()))
                    return true;
            }
            return false;
        }
    }
}
