namespace UIWidgets
{
	using System.IO;
	using UnityEngine;

	/// <summary>
	/// DrivesListView.
	/// </summary>
	public class DrivesListView : ListViewCustom<DrivesListViewComponentBase, FileSystemEntry>
	{
		/// <summary>
		/// FileListView
		/// </summary>
		[HideInInspector]
		public FileListView FileListView;

		/// <summary>
		/// The modal ID.
		/// </summary>
		[HideInInspector]
		protected InstanceID? DrivesModalKey;

		/// <summary>
		/// Drives hierarchy position.
		/// </summary>
		protected HierarchyPosition DrivesPosition;

		/// <summary>
		/// Parent canvas.
		/// </summary>
		[SerializeField]
		public RectTransform ParentCanvas;

		/// <summary>
		/// Is drives data loaded?
		/// </summary>
		[HideInInspector]
		protected bool DrivesLoaded;

		/// <summary>
		/// Load data.
		/// </summary>
		public void Load()
		{
			DataSource.BeginUpdate();
			DataSource.Clear();

			try
			{
				FileListView.ExceptionsView.Execute(GetDrives);
			}
			finally
			{
				DrivesLoaded = true;
				DataSource.EndUpdate();
			}
		}

		/// <summary>
		/// Toggle.
		/// </summary>
		public void Toggle()
		{
			if (DrivesModalKey != null)
			{
				Close();
			}
			else
			{
				Open();
			}
		}

		/// <summary>
		/// Open DrivesListView.
		/// </summary>
		public void Open()
		{
			if (!DrivesLoaded)
			{
				Load();
			}

			if (ParentCanvas == null)
			{
				ParentCanvas = UtilitiesUI.FindTopmostCanvas(transform);
			}

			DrivesModalKey = ModalHelper.Open(this, null, new Color(0, 0, 0, 0f), Close, ParentCanvas);
			DrivesPosition = HierarchyPosition.SetParent(transform, ParentCanvas);

			var selected = SelectedIndicesList;
			foreach (var index in selected)
			{
				Deselect(index);
			}

			gameObject.SetActive(true);
		}

		/// <summary>
		/// Close.
		/// </summary>
		public void Close()
		{
			ModalHelper.Close(ref DrivesModalKey);
			DrivesPosition.Restore();

			gameObject.SetActive(false);
		}

		/// <summary>
		/// Load drives list.
		/// </summary>
		protected virtual void GetDrives()
		{
#if !NETFX_CORE
			var drives = Directory.GetLogicalDrives();
			for (int i = 0; i < drives.Length; i++)
			{
				var item = new FileSystemEntry(drives[i], drives[i], false);
				DataSource.Add(item);
			}
#endif
		}

		/// <summary>
		/// Process the destroy event.
		/// </summary>
		protected override void OnDestroy()
		{
			DrivesPosition.ParentDestroyed();

			base.OnDestroy();
		}

		#if UNITY_EDITOR
		/// <summary>
		/// Validate this instance.
		/// </summary>
		protected override void OnValidate()
		{
			base.OnValidate();

			if (ParentCanvas == null)
			{
				ParentCanvas = UtilitiesUI.FindTopmostCanvas(transform);
			}
		}

		/// <summary>
		/// Reset this instance.
		/// </summary>
		protected override void Reset()
		{
			base.Reset();

			if (ParentCanvas == null)
			{
				ParentCanvas = UtilitiesUI.FindTopmostCanvas(transform);
			}
		}
		#endif
	}
}