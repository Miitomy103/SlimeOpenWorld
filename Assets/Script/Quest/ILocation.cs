using UnityEngine;

public interface ILocation
{
    string LocationID { get; }
    Transform Transform { get; }
}
