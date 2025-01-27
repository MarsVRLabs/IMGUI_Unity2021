using UnityEngine.Assertions;
using UnityEngine.Rendering;
#if HAS_URP
using UnityEngine.Rendering.Universal;
#endif

namespace ImGuiNET.Unity
{
    internal static class RenderUtils
    {
        public enum RenderType
        {
            Mesh = 0,
            Procedural = 1,
        }

        public static IImGuiRenderer Create(RenderType type, ShaderResourcesAsset shaders, TextureManager textures)
        {
            Assert.IsNotNull(shaders, "Shaders not assigned.");
            return type switch
            {
                RenderType.Mesh => new ImGuiRendererMesh(shaders, textures),
                RenderType.Procedural => new ImGuiRendererProcedural(shaders, textures),
                _ => null
            };
        }

        // ReSharper disable once InconsistentNaming
        public static bool IsUsingURP()
        {
            RenderPipelineAsset currentRP = GraphicsSettings.currentRenderPipeline;
#if HAS_URP
            return currentRP is UniversalRenderPipelineAsset;
#else
            return false;
#endif
        }

        public static CommandBuffer GetCommandBuffer(string name)
        {
#if HAS_URP
            return CommandBufferPool.Get(name);
#else
            return new CommandBuffer { name = name };
#endif
        }

        public static void ReleaseCommandBuffer(CommandBuffer cmd)
        {
#if HAS_URP
            CommandBufferPool.Release(cmd);
#else
            cmd.Release();
#endif
        }
    }
}
