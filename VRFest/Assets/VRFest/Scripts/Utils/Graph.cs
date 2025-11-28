using UnityEngine;

public class Graph : MonoBehaviour
{
    [Header("Grid Settings")]
    public float scale = 1f;
    public GameObject XDivision;
    public int AmountXDivisions = 10;
    public GameObject YDivision;
    public int AmountYDivisions = 10;

    [Header("Graph Settings")]
    public GameObject Point;
    
    private GameObject[] points;
    private Vector3[] vertices;
    private LineRenderer lineRenderer;

    void Start()
    {
        CreateCoordinateSystem();
    }

    // Создание системы координат
    public void CreateCoordinateSystem()
    {
        // Создаем родительский объект для сетки
        GameObject gridParent = new GameObject("CoordinateSystem");
        gridParent.transform.SetParent(transform);

        // Создаем деления по оси X (положительные и отрицательные)
        for (int i = -AmountXDivisions; i <= AmountXDivisions; i++)
        {
            if (i == 0) continue; // Пропускаем центр
            GameObject division = Instantiate(XDivision, new Vector3(i * scale, 0, 0), Quaternion.identity);
            division.transform.SetParent(gridParent.transform);
            division.name = $"XDivision_{i}";
        }

        // Создаем деления по оси Y (положительные и отрицательные)
        for (int i = -AmountYDivisions; i <= AmountYDivisions; i++)
        {
            if (i == 0) continue; // Пропускаем центр
            GameObject division = Instantiate(YDivision, new Vector3(0, i * scale, 0), Quaternion.identity);
            division.transform.SetParent(gridParent.transform);
            division.name = $"YDivision_{i}";
        }
    }

    // Инициализация графика
    public void DrawGraph(Vector3[] pointsArray)
    {
        vertices = pointsArray;
        points = new GameObject[vertices.Length];
        
        // Получаем или добавляем LineRenderer
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        
        // Настройка LineRenderer
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.blue;
        lineRenderer.endColor = Color.blue;
        lineRenderer.useWorldSpace = false;

        // Создаем родительский объект для точек
        GameObject pointsParent = new GameObject("GraphPoints");
        pointsParent.transform.SetParent(transform);

        // Создаем точки
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = Instantiate(Point, Vector3.zero, Quaternion.identity);
            points[i].transform.SetParent(pointsParent.transform);
            points[i].name = $"Point_{i}";
        }

        // Позиционируем точки
        LocateGraph(pointsArray);
    }

    // Позиционирование точек графика
    public void LocateGraph(Vector3[] pointsArray)
    {
        if (points == null || vertices == null || pointsArray.Length != vertices.Length)
        {
            Debug.LogError("Graph not properly initialized or array size mismatch");
            return;
        }

        // Обновляем вершины с учетом масштаба
        for (int i = 0; i < pointsArray.Length; i++)
        {
            vertices[i] = new Vector3(pointsArray[i].x * scale, pointsArray[i].y * scale, 0);
        }

        // Позиционируем точки
        for (int i = 0; i < pointsArray.Length; i++)
        {
            if (points[i] != null)
                points[i].transform.localPosition = vertices[i];
        }

        // Обновляем LineRenderer
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = vertices.Length;
            lineRenderer.SetPositions(vertices);
        }
    }

    // Обновление графика с новыми данными
    public void UpdateGraph(Vector3[] newPointsArray)
    {
        if (points == null)
        {
            DrawGraph(newPointsArray);
        }
        else
        {
            LocateGraph(newPointsArray);
        }
    }

    // Очистка графика
    public void ClearGraph()
    {
        // Удаляем точки
        if (points != null)
        {
            foreach (GameObject point in points)
            {
                if (point != null)
                    Destroy(point);
            }
            points = null;
        }

        // Удаляем LineRenderer
        if (lineRenderer != null)
        {
            Destroy(lineRenderer);
            lineRenderer = null;
        }

        // Удаляем вершины
        vertices = null;

        // Удаляем старую систему координат
        Transform coordSystem = transform.Find("CoordinateSystem");
        if (coordSystem != null)
            Destroy(coordSystem.gameObject);
    }

    // Перестроение всего графика
    public void RebuildGraph(Vector3[] pointsArray)
    {
        ClearGraph();
        CreateCoordinateSystem();
        DrawGraph(pointsArray);
    }

    // Пример использования с математической функцией
    public void DrawFunction(System.Func<float, float> function, float startX, float endX, int resolution = 50)
    {
        Vector3[] functionPoints = new Vector3[resolution];
        float step = (endX - startX) / (resolution - 1);

        for (int i = 0; i < resolution; i++)
        {
            float x = startX + i * step;
            float y = function(x);
            functionPoints[i] = new Vector3(x, y, 0);
        }

        DrawGraph(functionPoints);
    }

    [ContextMenu("Draw Sine Wave")]
    public void DrawSineWaveExample()
    {
        DrawFunction(x => Mathf.Sin(x), -Mathf.PI, Mathf.PI, 100);
    }

    [ContextMenu("Draw Parabola")]
    public void DrawParabolaExample()
    {
        DrawFunction(x => x * x, -2f, 2f, 50);
    }

    [ContextMenu("Clear Graph")]
    public void ClearGraphCommand()
    {
        ClearGraph();
    }
}