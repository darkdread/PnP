using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TableManager : MonoBehaviour
{
	[Header("Table Settings")]
	[HideInInspector] RectTransform tableManager; //Gets the Rect Transform of the Table Manager
	[SerializeField] int columns, rows;
	[SerializeField] float tableWidth, cellHeight;
	[SerializeField] float[] colPercentages;

	[Header("Table Rows and Cells")]
	List<RectTransform> tableRows;
	TextMeshProUGUI[,] tableCells;

	[Header("Prefabs for Instantiation")]
	[Tooltip("Cell Prefab Associated with this Package. The Cell Prefab should consist of a TextMeshProUGUI Component")]
	[SerializeField] TextMeshProUGUI cellPrefab;

	[Header("Row Color")]
	[SerializeField] bool useAlternatingColors; //Check if you want the Rows to be of Alternating Colors
	[SerializeField] Color color0 = Color.white;
	[SerializeField] Color color1 = Color.grey;

	private void Awake()
	{
		tableManager = GetComponent<RectTransform>();
		CreateTable(rows, columns, tableWidth, cellHeight, colPercentages);
		//CreateTable(rows, columns, tableWidth, cellHeight);
		//SetColSize(colPercentages);
		SetColData(2, 1, "Test SetData()");
	}

	/// <summary>
	/// Creates a Table according to the given size and automatically resizes the Table Columns from the percentages passed.
	/// </summary>
	/// <param name="row">Sets the number of Rows the Table will have</param>
	/// <param name="col">Sets the number of Columns the Table will have</param>
	/// <param name="tableWidth">Sets the total width of the Table</param>
	/// <param name="cellHeight">Sets the height of each Cell</param>
	/// <param name="colPercent">Refers to the Percentage of the Table Width meant for the sizing of each Cell</param>
	void CreateTable(int row, int col, float tableWidth, float cellHeight, float[] colPercent)
	{
		if (colPercent.Length != col)
		{
			Debug.LogError("The Array Length for the Percentages does not add up to the Column Length");
			return;
		}

		if (tableManager) tableManager.sizeDelta = new Vector2(tableWidth, cellHeight * row); //Declare the Size of the Table Manager so that it Encompasses the Table Nicely
		tableRows = new List<RectTransform>();
		tableCells = new TextMeshProUGUI[row, col]; //Declare the Number of Rows and Columns to the Cells

		for (int i = 0; i < tableCells.GetLength(0); i++)
		{
			//RectTransform cellRow = InstantiateRectTransform("Row " + i, tableManager);
			Color rowColor = useAlternatingColors ? i % 2 == 0 ? color0 : color1 : color0;
			RectTransform cellRow = InstantiateImage("Row " + i, rowColor, tableManager).rectTransform;

			//Set Pivots and Anchors
			cellRow.pivot = new Vector2(0, 1);
			cellRow.anchorMin = new Vector2(0, 1);
			cellRow.anchorMax = new Vector2(0, 1);

			//Adjust Size of Row to Encompass all the Cells Nicely
			cellRow.sizeDelta = new Vector2(tableWidth, cellHeight);
			cellRow.anchoredPosition = new Vector2(0, cellHeight * -i);
			tableRows.Add(cellRow);

			//Store the Percentage of Column Sizes so as to know how much to shift the CellRect Horizontally
			float totalPercent = 0;

			for (int j = 0; j < tableCells.GetLength(1); j++)
			{
				TextMeshProUGUI cell = Instantiate(cellPrefab, cellRow);
				RectTransform cellRect = cell.rectTransform;

				//Set Pivots and Anchors (Top Left). For Resizing and Positioning Purposes
				cellRect.pivot = new Vector2(0, 1);
				cellRect.anchorMin = new Vector2(0, 1);
				cellRect.anchorMax = new Vector2(0, 1);

				//Adjust Size of Cell
				cellRect.sizeDelta = new Vector2(colPercent[j] / 100 * tableWidth, cellHeight);

				//Set Cell Position in Table
				float xPos = totalPercent / 100 * tableWidth;
				cellRect.anchoredPosition = new Vector2(xPos, 0);

				//Calculate Percentage Accumulated
				totalPercent += colPercent[j];

				tableCells[i, j] = cell;
			}
		}

		print("Table Created and Resized");
	}

	/// <summary>
	/// Creates a Table according to the given size.
	/// </summary>
	/// <param name="row">Sets the number of Rows the Table will have</param>
	/// <param name="col">Sets the number of Columns the Table will have</param>
	/// <param name="tableWidth">Sets the total width of the Table</param>
	/// <param name="cellHeight">Sets the height of each Cell</param>
	void CreateTable(int row, int col, float tableWidth, float cellHeight)
	{
		if (tableManager) tableManager.sizeDelta = new Vector2(tableWidth, cellHeight * row); //Declare the Size of the Table Manager so that it Encompasses the Table Nicely
		tableRows = new List<RectTransform>();
		tableCells = new TextMeshProUGUI[row, col]; //Declare the Number of Rows and Columns to the Cells
		float estCellWidth = tableWidth / col; //Split All the Cell Columns Evenly First

		for (int i = 0; i < tableCells.GetLength(0); i++)
		{
			//RectTransform cellRow = InstantiateRectTransform("Row " + i, tableManager);
			Color rowColor = useAlternatingColors ? i % 2 == 0 ? color0 : color1 : color0;
			RectTransform cellRow = InstantiateImage("Row " + i, rowColor, tableManager).rectTransform;

			//Set Pivots and Anchors
			cellRow.pivot = new Vector2(0, 1);
			cellRow.anchorMin = new Vector2(0, 1);
			cellRow.anchorMax = new Vector2(0, 1);

			//Adjust Size of Row to Encompass all the Cells Nicely
			cellRow.sizeDelta = new Vector2(tableWidth, cellHeight);
			cellRow.anchoredPosition = new Vector2(0, cellHeight * -i);
			tableRows.Add(cellRow);

			for (int j = 0; j < tableCells.GetLength(1); j++)
			{
				TextMeshProUGUI cell = Instantiate(cellPrefab, cellRow);
				RectTransform cellRect = cell.rectTransform;

				//Set Pivots and Anchors (Top Left). For Resizing and Positioning Purposes
				cellRect.pivot = new Vector2(0, 1);
				cellRect.anchorMin = new Vector2(0, 1);
				cellRect.anchorMax = new Vector2(0, 1);

				//Adjust Size of Cell
				cellRect.sizeDelta = new Vector2(estCellWidth, cellHeight);

				//Set Cell Position in Table
				cellRect.anchoredPosition = new Vector2(estCellWidth * j, 0);

				tableCells[i, j] = cell;
			}
		}

		print("Table Created");
	}

	/// <summary>
	/// Resizes the Table Columns from the percentages passed.
	/// </summary>
	/// <param name="colPercent">Refers to the Percentage of the Table Width meant for the sizing of each Cell</param>
	void SetColSize(float[] colPercent)
	{
		int count = colPercent.Length;
		if (colPercent.Length != tableCells.GetLength(1))
		{
			Debug.LogError("The Array Length for the Percentages does not add up to the Column Length");
			return;
		}

		for (int i = 0; i < tableCells.GetLength(0); i++)
		{
			//Store the Percentage of Column Sizes so as to know how much to shift the CellRect Horizontally
			float totalPercent = 0;

			for (int j = 0; j < colPercent.Length; j++)
			{
				RectTransform cellRect = tableCells[i, j].rectTransform;

				//Set Size and Position of Cell Rect
				cellRect.sizeDelta = new Vector2(colPercent[j]/100 * tableWidth, cellHeight);

				//Set Cell Position in Table
				float xPos = totalPercent/100 * tableWidth;
				cellRect.anchoredPosition = new Vector2(xPos, cellRect.anchoredPosition.y);

				//Calculate Percentage Accumulated
				totalPercent += colPercent[j];
			}
		}

		print("Table Resized Successfully");
	}

	/// <summary>
	/// Sets the Data of a Specified Cell
	/// </summary>
	/// <param name="row">Row Index of Cell to be edited</param>
	/// <param name="col">Column Index of Cell to be edited</param>
	/// <param name="colString">New Data to insert into Cell</param>
	void SetColData(int row, int col, string colString)
	{
		tableCells[row, col].text = colString;
	}

	#region Component Instantiation
	/// <summary>
	/// Creates a new RectTransform without the reliance of a Prefab
	/// </summary>
	/// <param name="name">Name of the RectTransform</param>
	/// <param name="parent">Sets the Parent of the RectTransform</param>
	/// <returns></returns>
	RectTransform InstantiateRectTransform(string name, Transform parent = null)
	{
		GameObject rect = new GameObject(name);
		rect.transform.parent = parent;
		return rect.AddComponent<RectTransform>();
	}

	/// <summary>
	/// Creates a new RectTransform without the reliance of a Prefab
	/// </summary>
	/// <param name="name">Name of the Image</param>
	/// <param name="color">Color of the Image</param>
	/// <param name="parent">Sets the Parent of the Newly Created Image</param>
	/// <returns></returns>
	Image InstantiateImage(string name, Color color, Transform parent = null)
	{
		GameObject rect = new GameObject(name);
		rect.transform.parent = parent;
		Image img = rect.AddComponent<Image>();
		img.color = color;
		return img;
	}
	#endregion
}
