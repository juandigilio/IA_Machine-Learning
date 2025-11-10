using UnityEngine;

public class Scavenger : Agent
{
    public float Speed = 10.0f;
    public float RotSpeed = 20.0f;

    private Genome genome;
    private NeuralNetwork brain;
    protected GameObject nearMine;
    //protected GameObject goodMine;
    //protected GameObject badMine;
    private float[] inputs;

    private float fitness = 0;
    private float fitnessToGive = 0;

    public void SetBrain(Genome genome, NeuralNetwork brain)
    {
        this.genome = genome;
        this.brain = brain;
        inputs = new float[brain.InputsCount];
        OnReset();
    }

    private Vector3 GetDirToMine(GameObject mine)
    {
        return (mine.transform.position - this.transform.position).normalized;
    }

    private bool IsCloseToMine(GameObject mine)
    {
        return (this.transform.position - nearMine.transform.position).sqrMagnitude <= 2.0f;
    }

    private void SetForces(float leftForce, float rightForce, float dt)
    {
        Vector3 pos = this.transform.position;
        float rotFactor = Mathf.Clamp((rightForce - leftForce), -1.0f, 1.0f);

        float minRotation = 0.2f;

        if (rotFactor > -minRotation && rotFactor < minRotation)
        {
            fitnessToGive += 10f;
        }

        this.transform.rotation *= Quaternion.AngleAxis(rotFactor * RotSpeed * dt, Vector3.up);
        pos += this.transform.forward * Mathf.Abs(rightForce + leftForce) * 0.5f * Speed * dt;
        this.transform.position = pos;


        float abs = Mathf.Abs(rightForce + leftForce) * 0.5f;

        if (abs > 0.93f)
        {
            fitnessToGive += 100;
        }
    }

    public void Think(float dt)
    {
        OnThink(dt);

        if (IsCloseToMine(nearMine))
        {
            OnTakeMine(nearMine);
            PopulationManager.Instance.RelocateMine(nearMine);
        }
    }

    private void OnReset()
    {
        fitness = 1;
    }

    private void OnThink(float dt)
    {
        Vector3 dirToMine = GetDirToMine(nearMine);

        inputs[0] = dirToMine.x;
        inputs[1] = dirToMine.z;
        inputs[2] = transform.forward.x;
        inputs[3] = transform.forward.z;

        float[] output = brain.Synapsis(inputs);

        //Debug.Log($"Output 0: " + output[0]);
        //Debug.Log($"Output 1: " + output[1]);

        SetForces(output[0], output[1], dt);
        //Debug.Log($"DT: " + dt);
    }

    private void OnTakeMine(GameObject mine)
    {
        fitness += fitnessToGive;
        fitness *= 2;
        genome.fitness = fitness;
    }
}
