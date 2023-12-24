using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class LockRange
{
    public int startIndex;
    public int endIndex;
    public List<UnlockRequirement> requirements;
    public LockType lockType;
}

[System.Serializable]
public class UnlockRequirement
{
    public DiscColor color;
    public int requirement;
}

public class Stack : MonoBehaviour
{
    [Header("Spawn Data")]
    [SerializeField] List<StackSpawnData> spawnData;
    [SerializeField] DataList spawnDataList;

    [SerializeField] int spawnNumber;
    [SerializeField] DiscColor col;

    [Header("Disks Management")]
    [SerializeField] public List<Disc> discs;
    [SerializeField] Disc selectedDisc;
    [SerializeField] List<LockRange> locks;
    [SerializeField] public bool isStacking;


    [Header("VFX Management")]
    [SerializeField] public GameObject powerLaser;


    // Start is called before the first frame update
    void Start()
    {
        SpawnStack();
        StackManager.Instance.AddStack(this);
    }



    public void SpawnStack()
    {
        if(spawnDataList != null)
        foreach (StackSpawnData spd in spawnDataList.dataList)
        {
            for (int i = 0; i < spd.count; i++)
            {
                GameObject g = Instantiate(StackManager.Instance.GetDiscObject(spd.color), new Vector3(transform.position.x, StackManager.Instance.yBaseOffset+ transform.position.y + (StackManager.Instance.yOffset * discs.Count), transform.position.z), Quaternion.identity);
                g.GetComponent<Disc>().SetupDisc(this, discs.Count);
                discs.Add(g.GetComponent<Disc>());

                g.transform.SetParent(transform);
            }
        }

        SetupLocks();
    }

    public void SetupLocks()
    {
        foreach(LockRange lr in locks)
        {

        }
    }

    public void ResetSelection()
    {
        StackManager.Instance.selectedDisc = null;
        selectedDisc = null;
        foreach (Disc c in discs)
        {
            c.ResetSelection();
        }
    }

    public Vector3 GetDiscTransformPosition(Disc c)
    {
        return new Vector3(transform.position.x, StackManager.Instance.yBaseOffset + transform.position.y + (StackManager.Instance.yOffset * c.stackIndex), transform.position.z);
    }

    public void SendSelectedStackTo(Stack s)
    {

        int x = discs.Count - selectedDisc.stackIndex;
        int y = selectedDisc.stackIndex;
        for (int i = 0; i < x; i++)
        {
            discs[y].AddDiscTo(s);
        }

        s.CheckForStack();
    }

    // In your Stack class
    public IEnumerator SendSelectedStackToCoroutine(Stack targetStack)
    {
        float delayBetweenDiscs = 0.02f; // Set a delay between each disc animation
        int x = discs.Count - selectedDisc.stackIndex;
        int y = selectedDisc.stackIndex;

        for (int i = 0; i < x; i++)
        {
            Disc disc = discs[y];
            disc.AddDiscTo(targetStack); // Move the disc to the new stack
            yield return new WaitForSeconds(delayBetweenDiscs); // Wait for a bit before moving the next disc
        }

        targetStack.CheckForStack(); // Check for whatever you need to in the new stack
    }



    public void RemoveDisc(Disc d)
    {
        discs.Remove(d);
    }

    public int AddDisc(Disc d)
    {
        discs.Add(d);
        return discs.IndexOf(d);
    }


    //Mechanics
    //LOCK: Require specific disc to unlock
    //BARELL: Require number of discs of color to unlock

    //Lock stack range
    //If locked then cant interact BUT can place on top
    

    public bool CheckForStack()
    {
        int consecutiveCount = 1;
        for (int i = 1; i < discs.Count; i++)
        {
            // Compare current disc value with previous disc value
            if (discs[i].color == discs[i - 1].color && (!discs[i].isSpin && !discs[i-1].isSpin))
            {
                consecutiveCount++;

                if (discs.Count - 1 == i)
                {
                    if (consecutiveCount >= 3)
                    {

                        //Match all
                        Match(i, consecutiveCount, false);
                        return true;
                    }
                    else
                    {
                    }
                }
            }
            else
            {
                if (consecutiveCount >= 3)
                {
                    //Match all
                   
                    Match(i - 1, consecutiveCount, false);
                    return true;
                }
                else
                {
                }
                // Reset the count if they don't match                
                consecutiveCount = 1;
            }         
        }
        return false;
    }
    public Disc GetDiscOfColor(DiscColor c)
    {
        return discs.Find(x => x.color == c && x.isSpin == false);
    }
    //Also check for combo
    //Play VFX Also
    public void Match(int startIndex, int count, bool final)
    {

        // Main Sequence for popping and other stuff
        Sequence mySequence = DOTween.Sequence().PrependInterval(0.2f); // initial 0.2s delay
        Sequence rotateSequence = DOTween.Sequence();
        GameObject g = Instantiate(powerLaser, discs[startIndex - count + 1].transform.position * 1.2f, Quaternion.identity);
        isStacking = true;
        if (count >= 4)
        {
            UIManager.Instance.UpdateComboText(count);
            discs[startIndex-count+1].Wow();
           
            g.GetComponent<AutoRotate>().OrbitCenterTransform = discs[startIndex - count + 1].transform.position;
            g.GetComponent<AutoRotate>().enabled = true;
            g.GetComponent<AutoRotate>().Orbit(true);

        }
        else if (count == 1 && !final)
        {
            g.GetComponent<AutoRotate>().OrbitCenterTransform = discs[startIndex - count + 1].transform.position;
            g.GetComponent<AutoRotate>().enabled = true;
            g.GetComponent<AutoRotate>().Orbit(true);
        }
        else
        {
            Destroy(g);
        }        

        float f = 0.05f;

        List<Disc> toPop = new List<Disc>();
        DiscColor dc = discs[startIndex].color;

        for (int i = startIndex; i > startIndex - count; i--)
        {
            toPop.Add(discs[i]);
            discs[i].isSpin = true;

        }
   
        foreach (Disc d in toPop)
        {
            d.Highlight();
            d.transform.DOLocalRotate(new Vector3(0, 360, 0), 0.1f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental).SetDelay(f);
            f += 0.04f;
        }
    

        if(count!=1)
        mySequence.PrependInterval(0.15f);

      
        foreach (Disc e in toPop)
        {
            Disc currentDisc = e;
            float p = 1f;

            mySequence.AppendInterval(0.1f) // wait a bit to let 'em spin
                     .AppendCallback(() =>
                     {
                         SoundManager.Instance.Play(SoundManager.SoundType.Pop, p += 0.2f);
                         VibrationManager.Instance.PlayMedium();
                         currentDisc.PlayVFX();
                         currentDisc.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
                         {
                             currentDisc.Pop();
                             foreach (Disc c in discs)
                             {
                                 c.UpdateIndex(discs.FindIndex(x => x == c));
                             }
                             foreach (Disc c in discs)
                             {
                                 c.ResetSelection();
                             }


                         });
                     });
                                     
        }
       /* for (int i = startIndex; i > startIndex - count; i--)
        {
            float p = 1f;
            Disc currentDisc = discs[i];
            mySequence.AppendInterval(0.15f) // wait a bit to let 'em spin
                      .AppendCallback(() =>
                      {
                          SoundManager.Instance.Play(SoundManager.SoundType.Pop, p += 0.2f);
                          VibrationManager.Instance.PlayMedium();
                          currentDisc.PlayVFX();
                          currentDisc.transform.DOScale(Vector3.zero, 0.25f).OnComplete(() =>
                          {
                              currentDisc.Pop();
                              foreach (Disc c in discs)
                              {
                                  c.UpdateIndex(discs.FindIndex(x => x == c));
                              }
                              foreach (Disc c in discs)
                              {
                                  c.ResetSelection();
                              }


                          });
                      });

            
        }*/
        // After the sequence is complete, wait for 0.5 seconds then call CheckForStack again
        mySequence.AppendInterval(0.35f).OnComplete(() =>
        {
            if (count > 3)
            {
                Disc d = StackManager.Instance.PopRandomDiscColor(dc, false, this);
                if (d != null)
                {
                    g.GetComponent<AutoRotate>().enabled = false;
                    g.GetComponent<PowerLaser>().MoveTo(d, false);

                }
                else
                {
                    Destroy(g);
                }

            }

            if (count == 1 && !final)
            {
                Disc d = StackManager.Instance.PopRandomDiscColor(dc, true, this);
                if (d != null)
                {
                    g.GetComponent<AutoRotate>().enabled = false;
                    g.GetComponent<PowerLaser>().MoveTo(d, true);

                }
                else
                {
                    Destroy(g);
                }

            }
           
            
            isStacking = false;
            StackManager.Instance.CheckForStack(this);

            StackManager.Instance.CheckForWin();

        });

    }



    public void SelectDisc(int index)
    {
        if (StackManager.Instance.selectedDisc == null)
        {
            StackManager.Instance.selectedDisc = discs[index];
            selectedDisc = discs[index];
            SoundManager.Instance.Play(SoundManager.SoundType.Pop);

            if (index > 0)
            {
                for (int i = index-1; i >= 0; i--)
                {
                    if (discs[i].color == selectedDisc.color)
                    {
                        discs[i].SelectDisc();
                        if (i == 0)
                        {
                            selectedDisc = discs[i];
                            StackManager.Instance.selectedDisc = selectedDisc;
                            break;
                        }
                    }
                    else
                    {
                        selectedDisc = discs[i+1];
                        StackManager.Instance.selectedDisc = selectedDisc;
                        break;
                    }
                }
            }
            
            for (int i = index; i < discs.Count; i++)
            {
                discs[i].SelectDisc();
            }
        }

        else
        {
            if(StackManager.Instance.selectedDisc.currentStack == this)
            {
                if (TutorialManager.Instance != null)
                {
                    return;
                }
                ResetSelection();
            }
            else
            {

                if ( discs.Count == 0 || StackManager.Instance.selectedDisc.color == discs[discs.Count -1].color)
                {
                    if(discs.Count>0)
                    selectedDisc = discs[index];
                    //StackManager.Instance.selectedDisc.currentStack.SendSelectedStackTo(this);
                    StartCoroutine(StackManager.Instance.selectedDisc.currentStack.SendSelectedStackToCoroutine(this));
                    StackManager.Instance.selectedDisc = null;
                    selectedDisc = null;
                    SoundManager.Instance.Play(SoundManager.SoundType.Pop);
                    GameManager.Instance.AddMove(1);
                    if(TutorialManager.Instance!=null)
                    {
                        TutorialManager.Instance.PlayNext();
                    }
                    //Stack to other
                }
                else
                {
                    //Show negative VFX
                    GameManager.Instance.AddMove(1);
                    SoundManager.Instance.Play(SoundManager.SoundType.Error);
                    VibrationManager.Instance.PlayHeavy();
                    StackManager.Instance.selectedDisc.Blink();
                    discs[discs.Count-1].Blink();
                    StackManager.Instance.selectedDisc.currentStack.ResetSelection();
                }

            }
        }
    }
   
}
