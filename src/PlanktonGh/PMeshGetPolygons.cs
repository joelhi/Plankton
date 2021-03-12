using System;
using System.Collections.Generic;

using Grasshopper;
using Grasshopper.Kernel;
using Plankton;
using Rhino.Geometry;

namespace PlanktonGh
{
    public class PMeshGetPolygons : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public PMeshGetPolygons()
          : base("Plankton Get Polygons", "PlanktonGetPolygons",
            "Get the faces of a plankton mesh as Polygons",
            "Mesh", "Plankton")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new GH_PlanktonMeshParam(), "PMesh", "PMesh", "Plankton mesh to get polygons from", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Polygons", "plgns", "Plankton mesh as list of polygons", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            PlanktonMesh mesh = null;
            DA.GetData(0, ref mesh);
            DA.SetDataList(0, mesh.ToPolylines());
        }

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
            get { return new Guid("c4fef6ad-d1c5-4d00-918a-77849e43d2ad"); }
        }
    }
}
