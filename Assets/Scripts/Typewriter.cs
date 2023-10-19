using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class Typewriter : MonoBehaviour
{
	TMP_Text _tmpProText;
	string writer = string.Empty;

	[SerializeField] float delayBeforeStart = 0f;
	[SerializeField] float timeBtwChars = 0.1f;
	[SerializeField] string leadingChar = " |";
	[SerializeField] bool leadingCharBeforeDelay = false;
	[SerializeField] float overflowDelay = 0.1f;
	private Vector2 initialOffsetMin, initialOffsetMax;
	public IEnumerator DoBeforeTyping;
	public IEnumerator TypeWriterEnumerator;

	private bool appendingLine = false;

	// Use this for initialization
	void Start()
	{
		_tmpProText = GetComponent<TMP_Text>()!;
	}

	private void OnEnable()
    {
		Start();
		var rect = GetComponent<RectTransform>();
		initialOffsetMax = rect.offsetMax;
		initialOffsetMin = rect.offsetMin;
    }

    private void OnDisable()
    {
		var rect = GetComponent<RectTransform>();
		rect.offsetMax = initialOffsetMax;
		rect.offsetMin = initialOffsetMin;
    }

    /// <summary>
    /// Start (or restart) the typewriter with a new message.
    /// Any message currently typing will be immediately overwritten.
    /// </summary>
    /// <param name="message">The new message to type.</param>
    public void StartTyping(string message)
    {
		InterruptTyping(true);
		appendingLine = false;
		writer = message;
		TypeWriterEnumerator = TypeWriterTMP();
		StartCoroutine(TypeWriterEnumerator);
    }

	/// <summary>
	/// Adds a new line to the current message and starts or interrupts
	/// the typewriter.
	/// </summary>
	/// <param name="message"></param>
	public void AddLine(string message)
    {
		InterruptTyping(false);
		appendingLine = true;
		writer += '\n' + message;
		TypeWriterEnumerator = TypeWriterTMP();
		StartCoroutine(TypeWriterEnumerator);
	}

	/// <summary>
	/// Interrupts the typewriter and updates the text dependent on the <paramref name="clear"/> value. <br/>
	/// If true, the text will be cleared. Otherwise, the text will be set to the value of <see cref="writer"/>.
	/// </summary>
	/// <param name="clear">Determines whether the typewriter should be cleared or not.</param>
	public void InterruptTyping(bool clear = false)
	{
		StopAllCoroutines();
		_tmpProText.text = clear ? string.Empty : writer;
	}

	/// <summary>
	/// This function is the driver code for typing each character to the screen.
	/// </summary>
	/// <returns>An IEnumerator for Unity Coroutines.</returns>
	IEnumerator TypeWriterTMP()
	{
		if (DoBeforeTyping is not null)
		{
			yield return DoBeforeTyping;
			DoBeforeTyping = null;
		}
		string writer = appendingLine ? this.writer.Substring(_tmpProText.text.Length, 
			this.writer.Length - _tmpProText.text.Length) : this.writer;
		_tmpProText.text += leadingCharBeforeDelay ? leadingChar : string.Empty;
		var rectTransform = GetComponent<RectTransform>();
		// Tell Unity to come back to us after a few seconds...
		if (delayBeforeStart != 0)
			yield return new WaitForSeconds(delayBeforeStart);
		// Begin writing each character one-by-one to the screen.
		foreach (char c in writer)
		{
			// Remove the leading character we previously added, if any.
			if (_tmpProText.text.Length > 0)
			{
				_tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - leadingChar.Length); // O(n)
			}
			if (_tmpProText.isTextOverflowing) yield return AnimateOverflowScroll(rectTransform);
			// Add the next character and the leading characters.
			_tmpProText.text += c; // O(n+1)
			_tmpProText.text += leadingChar; // O(n+1)
			// O(2n+2) - can be optimized with StringBuilder.
			yield return new WaitForSeconds(timeBtwChars);
		}
		// If leading char is empty, then we are done. Tell unity we are done via
		// yield break.
		if (leadingChar.Length == 0 || _tmpProText.text.Length == 0) yield break;
		_tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - leadingChar.Length);
	}

	IEnumerator AnimateOverflowScroll(RectTransform rectTransform)
	{
		while (_tmpProText.isTextOverflowing)
        {
			var offset = 10 * overflowDelay;
			float duration = 0;
			while (duration < overflowDelay)
			{
				duration += Time.deltaTime;
				rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, 
					rectTransform.offsetMax.y + offset * overflowDelay);
				yield return new WaitForEndOfFrame();
			}
		}
	}
}