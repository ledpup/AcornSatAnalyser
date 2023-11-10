﻿public struct Coordinates
{
    public Coordinates(float lat, float lng)
    {
        Latitude = lat;
        Longitude = lng;
        Elevation = 0;
    }

    public float Latitude { get; set; }
    public float Longitude { get; set; }
    public float? Elevation { get; set; }

    public readonly bool Equals(Coordinates obj) => Latitude == obj.Latitude && Longitude == obj.Longitude && Elevation == obj.Elevation;
    public static bool operator ==(Coordinates lhs, Coordinates rhs) => lhs.Equals(rhs);
    public static bool operator !=(Coordinates lhs, Coordinates rhs) => !(lhs == rhs);

    public override readonly string ToString()
    {
        if (Elevation == null)
        {
            return $"{Math.Round(Latitude, 0)}° {Math.Round(Longitude, 0)}°";
        }
        return $"{Math.Round(Latitude, 0)}° {Math.Round(Longitude, 0)}° {Math.Round(Elevation.Value, 0)}m";
    }

    public string ToFriendlyString(bool prefix = false)
    {
        if (prefix)
        {
            if (Elevation == null)
            {
                return $"Lat {Math.Round(Latitude, 1)}° Long {Math.Round(Longitude, 1)}°";
            }
            return $"Lat {Math.Round(Latitude, 1)}° Long {Math.Round(Longitude, 1)}° Ele {Math.Round(Elevation.Value, 1)}m";
        }
        return ToString();
    }

    public override readonly bool Equals(object? obj) => Equals((Coordinates)obj!);

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }
}
