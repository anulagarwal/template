using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackManager : MonoBehaviour
{
    [Header("Disc Spawn Data")]
    [SerializeField] List<SpawnObject> discObjects;
    [SerializeField] public float yOffset;
    [SerializeField] public float yBaseOffset;

    [SerializeField] public float yRiseOffset;


    [Header("Stack/Disc Data Management")]
    [SerializeField] List<Stack> stacks;
    [SerializeField] List<Disc> discs;
    [SerializeField] public Disc selectedDisc;
    [SerializeField] public int totalDiscs;




    #region Singleton
    private static StackManager _instance;
    public static StackManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<StackManager>();
                if (_instance == null)
                {
                    GameObject singleton = new GameObject("GameManager");
                    _instance = singleton.AddComponent<StackManager>();
                }
            }
            return _instance;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddStack(Stack s)
    {
        stacks.Add(s);
        totalDiscs += s.discs.Count;
        UIManager.Instance.UpdateDiscs(totalDiscs);
    }

    public void CheckForStack(Stack s)
    {
        foreach (Stack st in stacks)
        {
            if (st.isStacking)
            {
                return;
            }
        }

        if (!s.CheckForStack())
        {
            foreach (Stack st in stacks)
            {
                if (st == s)
                {
                    continue;
                }

                if (st.CheckForStack())
                {
                    return;
                }
            }
        }
    }

    public Disc PopRandomDiscColor(DiscColor dc, bool final, Stack skipStack = null)
    {
        // First round of search, skipping the stack if specified
        foreach (Stack s in stacks)
        {
            if (s == skipStack)
            {
                continue; // Skip this iteration
            }

            Disc d = s.GetDiscOfColor(dc);
            if (d != null)
            {
                d.isSpin = true;
               // s.Match(d.stackIndex, 1, final);
                return d;
            }
        }

        // If we are here, no other stacks had what we needed.
        // So let's check the skipped stack if it was specified.
        if (skipStack != null)
        {
            Disc d = skipStack.GetDiscOfColor(dc);
            if (d != null)
            {
                d.isSpin = true;

                //skipStack.Match(d.stackIndex, 1, final);
                return d;
            }
        }

        return null;
    }


    public int CalculateTotalDiscs()
    {
        int z = 0;

        foreach(Stack s in stacks)
        {
            z += s.discs.Count;
        }

        totalDiscs = z;
        return totalDiscs;
    }
    public void CheckForWin()
    {
        bool isWin = false;

        UIManager.Instance.UpdateDiscs(CalculateTotalDiscs());

        foreach (Stack s in stacks)
        {
            if (s.discs.Count == 0)
            {
                isWin = true;

            }
            else
            {
                isWin = false;
                break;
            }
        }

        if (isWin)
        {
            GameManager.Instance.WinLevel();
        }
    }

    public GameObject GetDiscObject(DiscColor dc)
    {
        return discObjects.Find(x => x.col == dc).disc;
    }
}
