using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

using Plankton;

namespace PlanktonGh
{
    public class PMeshFromPolygons : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public PMeshFromPolygons()
          : base("Plankton From Polygons", "PlanktonFromPolygons",
            "PMeshFromPolygons description",
            "Mesh", "Plankton")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Polygons", "plgns", "Faces for PMesh as closed polygons / polylines", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new GH_PlanktonMeshParam(), "PMesh", "PMesh", "Generated Plankton mesh", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Curve> faces = new List<Curve>();
            DA.GetDataList(0, faces);

            List<Polyline> polys = new List<Polyline>();
            for (int i = 0; i < faces.Count; i++)
            {
                var tempCrv = faces[i];

                if (tempCrv is PolylineCurve && tempCrv.IsClosed)
                    polys.Add(((PolylineCurve)tempCrv).ToPolyline());
                else
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "One or more curves are not polylines. These have been ignored. Please Check.");
            }

            DA.SetData(0, PMeshFromPolylines(polys));
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("602a3a75-d39a-4936-8d6e-165631215cbd"); }
        }

        PlanktonMesh PMeshFromPolylines(List<Polyline> faces)
        {
            var pMesh = new PlanktonMesh();
            //add n-gon faces
            var verts = new List<Point3d>();
            for (int i = 0; i < faces.Count; i++)
            {
                var currFace = new List<int>();

                for (int j = 0; j < faces[i].Count - 1; j++)
                {
                    var currPt = faces[i].PointAt(j);
                    var id = verts.IndexOf(currPt);
                    if (id < 0)
                    {
                        //push a vertex to list
                        //push that index to current face
                        pMesh.Vertices.Add(currPt.X, currPt.Y, currPt.Z);
                        verts.Add(currPt);
                        currFace.Add(pMesh.Vertices.Count - 1);
                    }
                    else
                    {
                        //push this index to current face
                        currFace.Add(id);
                    }
                }

                pMesh.Faces.AddFace(currFace);
            }

            return pMesh;
        }
    }
}
