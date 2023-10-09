﻿namespace UIWidgets
{
	using UIWidgets.Styles;
	using UnityEngine;
	using UnityEngine.Events;
	using UnityEngine.UI;

	/// <summary>
	/// InputField with exposed functions to validate input.
	/// </summary>
	public class InputFieldExtended : InputField, IInputFieldExtended
	{
#if !(UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_3_OR_NEWER)
		/// <summary>
		/// Current InputField caret position (also selection tail).
		/// </summary>
		/// <value>The caret position.</value>
		int IInputFieldProxy.caretPosition
		{
			get
			{
				return base.caretPosition;
			}

			set
			{
				base.caretPosition = value;
			}
		}
#endif

		/// <summary>
		/// Function to validate input.
		/// </summary>
		public OnValidateInput Validation
		{
			get;
			set;
		}

		/// <summary>
		/// Function to process changed value.
		/// </summary>
		public UnityAction<string> ValueChanged
		{
			get;
			set;
		}

		/// <summary>
		/// The Unity Event to call when edit.
		/// </summary>
		/// <value>The OnValueChange.</value>
		UnityEvent<string> IInputFieldProxy.onValueChanged
		{
			get
			{
#if UNITY_5_3 || UNITY_5_3_OR_NEWER
				return onValueChanged;
#else
				return onValueChange;
#endif
			}
		}

		/// <summary>
		/// Function to process end edit.
		/// </summary>
		public UnityAction<string> ValueEndEdit
		{
			get;
			set;
		}

		/// <summary>
		/// The Unity Event to call when editing has ended.
		/// </summary>
		/// <value>The OnEndEdit.</value>
		UnityEvent<string> IInputFieldProxy.onEndEdit
		{
			get
			{
				return onEndEdit;
			}
		}

		/// <summary>
		/// Current value.
		/// </summary>
		public string Value
		{
			get
			{
				return text;
			}

			set
			{
				currentPauseValidation = !ValidationOnSetValue;
				#if UNITY_5_3_4 || UNITY_5_3_OR_NEWER
				if (text != value)
				{
					text = value;
					UpdateLabel();
				}
				#else
				text = value;
				#endif
				currentPauseValidation = false;
			}
		}

		/// <summary>
		/// Is InputField has rich text support?
		/// </summary>
		public bool richText
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Start selection position.
		/// </summary>
		public int SelectionStart
		{
			get
			{
#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_3_OR_NEWER
				return Mathf.Min(selectionAnchorPosition, selectionFocusPosition);
#else
				return Mathf.Min(caretSelectPos, caretPosition);
#endif
			}
		}

		/// <summary>
		/// End selection position.
		/// </summary>
		public int SelectionEnd
		{
			get
			{
#if UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_3_OR_NEWER
				return Mathf.Max(selectionAnchorPosition, selectionFocusPosition);
#else
				return Mathf.Max(caretSelectPos, caretPosition);
#endif
			}
		}

		/// <summary>
		/// Text component.
		/// </summary>
		Graphic IInputFieldProxy.textComponent
		{
			get
			{
				return textComponent;
			}

			set
			{
				textComponent = value as Text;
			}
		}

		/// <summary>
		/// Continue or pause validation on set value.
		/// </summary>
		[SerializeField]
		public bool ValidationOnSetValue = false;

		bool currentPauseValidation;

		static char NoValidation(string text, int charIndex, char addedChar)
		{
			return addedChar;
		}

		static void DoNothing(string value)
		{
		}

		/// <summary>
		/// Start this instance.
		/// </summary>
		protected override void Start()
		{
			base.Start();

			if (Validation == null)
			{
				Validation = NoValidation;
			}

			if (ValueChanged == null)
			{
				ValueChanged = DoNothing;
			}

			if (ValueEndEdit == null)
			{
				ValueEndEdit = DoNothing;
			}

			onValidateInput = ProcessValidation;

#if UNITY_5_3 || UNITY_5_3_OR_NEWER
			onValueChanged.AddListener(ProcessValueChanged);
#else
			onValueChange.AddListener(ProcessValueChanged);
#endif
			onEndEdit.AddListener(ProcessValueEndEdit);
		}

		char ProcessValidation(string validateText, int charIndex, char addedChar)
		{
			return currentPauseValidation
				? addedChar
				: Validation(validateText, charIndex, addedChar);
		}

		void ProcessValueChanged(string value)
		{
			ValueChanged(value);
		}

		void ProcessValueEndEdit(string value)
		{
			ValueEndEdit(value);
		}

		/// <summary>
		/// Determines whether InputField instance is null.
		/// </summary>
		/// <returns><c>true</c> if InputField instance is null; otherwise, <c>false</c>.</returns>
		public bool IsNull()
		{
			return this != null;
		}

		/// <summary>
		/// Determines whether this lineType is LineType.MultiLineNewline.
		/// </summary>
		/// <returns><c>true</c> if lineType is LineType.MultiLineNewline; otherwise, <c>false</c>.</returns>
		public bool IsMultiLineNewline()
		{
			return lineType == LineType.MultiLineNewline;
		}

#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0
		/// <summary>
		/// Move caret to end.
		/// </summary>
		public void MoveToEnd()
		{
			MoveTextStart(false);
			MoveTextEnd(false);
		}
#endif

		/// <summary>
		/// Set focus to InputField.
		/// </summary>
		public void Focus()
		{
			ActivateInputField();
			Select();
		}

		/// <summary>
		/// Remove callbacks.
		/// </summary>
		protected override void OnDestroy()
		{
			base.OnDestroy();
#if UNITY_5_3 || UNITY_5_3_OR_NEWER
			onValueChanged.RemoveListener(ProcessValueChanged);
#else
			onValueChange.RemoveListener(ProcessValueChanged);
#endif
			onEndEdit.RemoveListener(ProcessValueEndEdit);
		}

		/// <inheritdoc/>
		public virtual void SetStyle(StyleSpinner styleSpinner, Style style)
		{
			styleSpinner.InputText.ApplyTo(textComponent, true);

			if (placeholder != null)
			{
				styleSpinner.InputPlaceholder.ApplyTo(placeholder.gameObject, true);
			}
		}

		/// <inheritdoc/>
		public virtual void GetStyle(StyleSpinner styleSpinner, Style style)
		{
			styleSpinner.InputText.GetFrom(textComponent, true);

			if (placeholder != null)
			{
				styleSpinner.InputPlaceholder.GetFrom(placeholder.gameObject, true);
			}
		}
	}
}