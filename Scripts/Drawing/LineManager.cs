using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    public List<LineRenderer> lines = new List<LineRenderer>();
    public LineRenderer lastRemoved = null;
    
    // Start is called before the first frame update
    void Start()
    {
        lines.Clear();
        lastRemoved = null;
    }

    public void AddLine(LineRenderer newLine)
    {
        lines.Add(newLine);
    }

    public void UndoLine()
    {
        if (lines.Count > 0)
        {
            if (lastRemoved)
            {
                Destroy(lastRemoved.gameObject); 
            }
            lastRemoved = lines[lines.Count-1];
            lines.Remove(lines[lines.Count - 1]);
            lastRemoved.GetComponent<Renderer>().enabled = false;
        }
    }

    public void RedoLine()
    {
        if (lastRemoved)
        {
            lines.Add(lastRemoved);
            lastRemoved.GetComponent<Renderer>().enabled = true;
            lastRemoved = null;
        }
    }
    
}
