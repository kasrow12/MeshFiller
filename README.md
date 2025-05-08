# Bezier Surface Renderer
Windows Forms application written in C# to visualize 3D Bezier surfaces. It features a custom rendering pipeline where triangles forming the surface are rasterized from scratch, pixel by pixel, using a scanline algorithm. The renderer also implements a Phong lighting model to simulate how light interacts with the surfaces, allowing for adjustments to material properties and light characteristics.


## Features

*   **Load Bezier Surface:** Load surface control points from a `.txt` file (expects 16 lines, each with 3 space-separated float values for X, Y, Z coordinates).
*   **Interactive Rotation:** Rotate the surface in 3D space using Alpha (X-axis) and Beta (Y-axis) angle sliders.
*   **Triangulation Control:**
    *   Adjust the density (resolution) of the generated mesh.
    *   Toggle the visibility of the triangulation lines.
*   **Lighting:**
    *   Phong lighting model.
    *   Adjust diffuse (kd), specular (ks), and shininess (m) coefficients.
    *   Change the light color.
    *   Animate the light source, making it orbit around the Z-axis.
    *   Adjust the Z-position of the light source.
*   **Object Appearance:**
    *   **Solid Color:** Apply a solid color to the surface.
    *   **Texture Mapping:** Apply an image texture to the surface.
    *   **Normal Mapping:** Apply a normal map for enhanced surface detail.
*   **View Controls:**
    *   **Scale:** Zoom in or out on the surface.
    *   **Z-Buffer:** Toggle the use of a Z-buffer for correct depth rendering.

## Screenshots
![rock](https://github.com/user-attachments/assets/7631e75a-3e5e-4567-8dfc-c495195ab633)
![mesh](https://github.com/user-attachments/assets/f436efe3-254e-49d9-9b1a-c99660a2b4eb)
![red](https://github.com/user-attachments/assets/6eeb59f0-3fba-435e-a57d-8753bdab2d1a)
![bezier](https://github.com/user-attachments/assets/85f7b7e9-0d9f-437b-af2c-cf18b25f19ff)
