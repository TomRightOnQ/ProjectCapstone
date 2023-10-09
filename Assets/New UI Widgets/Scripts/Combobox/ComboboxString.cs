namespace UIWidgets
{
	/// <summary>
	/// Combobox.
	/// </summary>
	public class ComboboxString : ComboboxCustom<ListViewString, ListViewStringItemComponent, string>
	{
		/// <inheritdoc/>
		protected override void InitCustomWidgets()
		{
			if (ListView != null)
			{
				var data = ListView.GetComponent<ListViewStringDataFile>();
				data.Init();
			}
		}
	}
}