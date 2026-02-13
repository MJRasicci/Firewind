/**
 * @license MIT
 * tailwind-scrollbar v4.0.2
 * https://github.com/adoxography/tailwind-scrollbar
 */
var __getOwnPropNames = Object.getOwnPropertyNames;
var __commonJS = (cb, mod) => function __require() {
  return mod || (0, cb[__getOwnPropNames(cb)[0]])((mod = { exports: {} }).exports, mod), mod.exports;
};

// node_modules/tailwindcss/dist/plugin.js
var require_plugin = __commonJS({
  "node_modules/tailwindcss/dist/plugin.js"(exports, module) {
    "use strict";
    function u(i, n) {
      return { handler: i, config: n };
    }
    u.withOptions = function(i, n = () => ({})) {
      function t(o) {
        return { handler: i(o), config: n(o) };
      }
      return t.__isOptionsFunction = true, t;
    };
    var g = u;
    module.exports = g;
  }
});

// node_modules/tailwindcss/dist/flatten-color-palette.js
var require_flatten_color_palette = __commonJS({
  "node_modules/tailwindcss/dist/flatten-color-palette.js"(exports, module) {
    "use strict";
    function n(r) {
      let i = {};
      for (let [e, t] of Object.entries(r ?? {})) if (e !== "__CSS_VALUES__") if (typeof t == "object" && t !== null) for (let [s, l] of Object.entries(n(t))) i[`${e}${s === "DEFAULT" ? "" : `-${s}`}`] = l;
      else i[e] = t;
      if ("__CSS_VALUES__" in r) for (let [e, t] of Object.entries(r.__CSS_VALUES__)) Number(t) & 4 || (i[e] = r[e]);
      return i;
    }
    module.exports = n;
  }
});

// src/typedefs.js
var require_typedefs = __commonJS({
  "src/typedefs.js"(exports) {
    exports.unused = {};
  }
});

// src/helpers.js
var require_helpers = __commonJS({
  "src/helpers.js"(exports, module) {
    var importDefault = (mod) => mod && mod.__esModule ? mod.default : mod;
    module.exports = {
      importDefault
    };
  }
});

// src/utilities.js
var require_utilities = __commonJS({
  "src/utilities.js"(exports, module) {
    var flattenColorPaletteImport = require_flatten_color_palette();
    var typedefs = require_typedefs();
    var { importDefault } = require_helpers();
    var flattenColorPalette = importDefault(flattenColorPaletteImport);
    var COMPONENTS = ["track", "thumb", "corner"];
    var scrollbarProperties = (properties, preferPseudoElements) => {
      if (preferPseudoElements) {
        return {
          "@supports (-moz-appearance:none)": properties
        };
      }
      return properties;
    };
    var addBaseStyles = ({ addBase }, preferredStrategy) => {
      addBase({
        "*": scrollbarProperties({
          "scrollbar-color": "initial",
          "scrollbar-width": "initial"
        }, preferredStrategy === "pseudoelements")
      });
    };
    var generateBaseUtilities = () => ({
      ...Object.fromEntries(COMPONENTS.map((component) => {
        const base = `&::-webkit-scrollbar-${component}`;
        return [
          [base, {
            "background-color": `var(--scrollbar-${component})`,
            "border-radius": `var(--scrollbar-${component}-radius)`
          }]
        ];
      }).flat())
    });
    var generateScrollbarSizeUtilities = ({ preferPseudoElements }) => ({
      ".scrollbar": {
        ...generateBaseUtilities(),
        ...scrollbarProperties({
          "scrollbar-width": "auto",
          "scrollbar-color": "var(--scrollbar-thumb, initial) var(--scrollbar-track, initial)"
        }, preferPseudoElements),
        "&::-webkit-scrollbar": {
          display: "block",
          width: "var(--scrollbar-width, 16px)",
          height: "var(--scrollbar-height, 16px)"
        }
      },
      ".scrollbar-thin": {
        ...generateBaseUtilities(),
        ...scrollbarProperties({
          "scrollbar-width": "thin",
          "scrollbar-color": "var(--scrollbar-thumb, initial) var(--scrollbar-track, initial)"
        }, preferPseudoElements),
        "&::-webkit-scrollbar": {
          display: "block",
          width: "8px",
          height: "8px"
        }
      },
      ".scrollbar-none": {
        ...scrollbarProperties({
          "scrollbar-width": "none"
        }, preferPseudoElements),
        "&::-webkit-scrollbar": {
          display: "none"
        }
      }
    });
    var toColorValue = (maybeFunction) => typeof maybeFunction === "function" ? maybeFunction({}) : maybeFunction;
    var addColorUtilities = ({ matchUtilities, theme }) => {
      const themeColors = flattenColorPalette(theme("colors"));
      const colors = Object.fromEntries(
        Object.entries(themeColors).map(([k, v]) => [k, toColorValue(v)])
      );
      COMPONENTS.forEach((component) => {
        matchUtilities(
          {
            [`scrollbar-${component}`]: (value) => ({
              [`--scrollbar-${component}`]: toColorValue(value)
            })
          },
          {
            values: colors,
            type: "color"
          }
        );
      });
    };
    var addRoundedUtilities = ({ theme, matchUtilities }) => {
      COMPONENTS.forEach((component) => {
        matchUtilities(
          {
            [`scrollbar-${component}-rounded`]: (value) => ({
              [`--scrollbar-${component}-radius`]: value
            })
          },
          {
            values: theme("borderRadius")
          }
        );
      });
    };
    var addBaseSizeUtilities = ({ addUtilities }, preferredStrategy) => {
      addUtilities(generateScrollbarSizeUtilities({
        preferPseudoElements: preferredStrategy === "pseudoelements"
      }));
    };
    var addSizeUtilities = ({ matchUtilities, theme }) => {
      ["width", "height"].forEach((dimension) => {
        matchUtilities({
          [`scrollbar-${dimension[0]}`]: (value) => ({
            [`--scrollbar-${dimension}`]: value
          })
        }, {
          values: theme(dimension)
        });
      });
    };
    module.exports = {
      addBaseStyles,
      addBaseSizeUtilities,
      addColorUtilities,
      addRoundedUtilities,
      addSizeUtilities
    };
  }
});

// src/variants.js
var require_variants = __commonJS({
  "src/variants.js"(exports, module) {
    var addVariants = ({ addVariant }) => {
      addVariant("scrollbar-hover", "&::-webkit-scrollbar-thumb:hover");
      addVariant("scrollbar-track-hover", "&::-webkit-scrollbar-track:hover");
      addVariant("scrollbar-corner-hover", "&::-webkit-scrollbar-corner:hover");
      addVariant("scrollbar-active", "&::-webkit-scrollbar-thumb:active");
      addVariant("scrollbar-track-active", "&::-webkit-scrollbar-track:active");
    };
    module.exports = {
      addVariants
    };
  }
});

// src/index.js
var require_index = __commonJS({
  "src/index.js"(exports, module) {
    var plugin = require_plugin();
    var {
      addBaseStyles,
      addBaseSizeUtilities,
      addColorUtilities,
      addRoundedUtilities,
      addSizeUtilities
    } = require_utilities();
    var { addVariants } = require_variants();
    module.exports = plugin.withOptions((options = {}) => (tailwind) => {
      let preferredStrategy = options.preferredStrategy ?? options.preferredstrategy ?? "standard";
      if (preferredStrategy !== "standard" && preferredStrategy !== "pseudoelements") {
        console.warn("WARNING: tailwind-scrollbar preferredStrategy should be 'standard' or 'pseudoelements'");
        preferredStrategy = "standard";
      }
      addBaseStyles(tailwind, preferredStrategy);
      addBaseSizeUtilities(tailwind, preferredStrategy);
      addColorUtilities(tailwind);
      addVariants(tailwind);
      if (options.nocompatible) {
        addRoundedUtilities(tailwind);
        addSizeUtilities(tailwind);
      }
    });
  }
});
export default require_index();

/*
MIT License

Copyright (c) Graham Still <gstill92@gmail.com>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

