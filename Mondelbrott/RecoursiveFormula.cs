using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Mondelbrott
{
    class RecoursiveFormula
    {
        private int _maxIterations, _maxIterationsHalf, _colorStep, _colorCount;
        private double _maxRadiusSq;

        public RecoursiveFormula(int maxIterations, double maxRadius, int colorCount)
        {
            _maxIterations = maxIterations;
            _maxIterationsHalf = maxIterations / 2;
            _maxRadiusSq = maxRadius * maxRadius;
            _colorStep = _maxIterationsHalf / colorCount;
            _colorCount = colorCount;
        }

        /// <summary>
        /// Creating/destroying the dot class is quite slow. 
        /// It's faster to operate with coordinates only and use struct instead of classes, and not use additional function calls.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public int Test(Dot c)
        {
            int i = 0;
            double p = c.x, q = c.y, y0 = 0, x0 = 0, r2 = 0, x1, y1;

            for (i = 0; i < _maxIterations; i++)
            {
                x1 = x0 * x0 - y0 * y0 + p;
                y1 = 2 * x0 * y0 + q;
                r2 = x1 * x1 + y1 * y1;
                if (r2 >= _maxRadiusSq)
                {
                    break;
                }
                x0 = x1;
                y0 = y1;
            }

            var index = (int)((i - _maxIterationsHalf) / _colorStep); // gives 0...colorStep - 1  
            if (index <= 0)
            {
                return 0;
            }
            else if (index >= _colorCount)
            {
                return _colorCount - 1;
            }
            else
            {
                return index;
            }
        }
    }
}
