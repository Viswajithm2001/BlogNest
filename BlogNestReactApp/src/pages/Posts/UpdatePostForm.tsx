import React, { useState, useEffect } from "react";
import { useForm } from "react-hook-form";
import { getPostById, updatePost } from "../../services/post";
import { useNavigate, useParams } from "react-router-dom";

type UpdatePostFormData = {
    title: string;
    content: string;
    tags: string;
    image?: FileList;
    deleteImage?: boolean;
};

export default function UpdatePostForm() {
    const { id } = useParams<{ id: string }>();
    const navigate = useNavigate();
    const [error, setError] = useState<string | null>(null);
    const [currentImage, setCurrentImage] = useState<string | undefined>();

    const { register, handleSubmit, setValue, formState: { isSubmitting, errors } } = useForm<UpdatePostFormData>();

    useEffect(() => {
        async function fetchPost() {
            if (!id) return;
            try {
                const post = await getPostById(id);
                setValue("title", post.title);
                setValue("content", post.content);
                setValue("tags", post.tags.map(t => t.name).join(", "));
                setCurrentImage(post.imageUrl);
            } catch (err: any) {
                setError("Failed to load post");
            }
        }
        fetchPost();
    }, [id, setValue]);

    const onSubmit = async (data: UpdatePostFormData) => {
        if (!id) return;
        try {
            const tagsArray = data.tags.split(",").map(tag => tag.trim()).filter(tag => tag.length > 0);
            const imageFile = data.image?.[0];
            await updatePost(id, data.title, data.content, tagsArray, imageFile, data.deleteImage || false);
            navigate("/posts");
        } catch (err: any) {
            setError(err.response?.data?.message || "Failed to update post");
        }
    };

    return (
        <div className="max-w-md mx-auto mt-10 p-6 bg-white shadow rounded">
            <h2 className="text-2xl font-bold mb-4">Update Post</h2>

            {error && <p className="text-red-500 mb-3">{error}</p>}

            <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
                <div>
                    <label className="block mb-1">Title</label>
                    <input
                        {...register("title", { required: "Title is required" })}
                        className="w-full border px-3 py-2 rounded"
                    />
                    {errors.title && <p className="text-red-500">{errors.title.message}</p>}
                </div>

                <div>
                    <label className="block mb-1">Content</label>
                    <textarea
                        {...register("content", { required: "Content is required" })}
                        className="w-full border px-3 py-2 rounded"
                    />
                    {errors.content && <p className="text-red-500">{errors.content.message}</p>}
                </div>

                <div>
                    <label className="block mb-1">Tags (comma separated)</label>
                    <input
                        {...register("tags")}
                        className="w-full border px-3 py-2 rounded"
                    />
                </div>

                {currentImage && (
                    <div className="mb-3">
                        <p>Current Image:</p>
                        <img src={currentImage} alt="Current" className="max-h-40 mb-2" />
                        <label className="flex items-center gap-2">
                            <input type="checkbox" {...register("deleteImage")} />
                            Delete current image
                        </label>
                    </div>
                )}

                <div>
                    <label className="block mb-1">Upload New Image</label>
                    <input
                        type="file"
                        accept="image/*"
                        {...register("image")}
                        className="w-full border px-3 py-2 rounded"
                    />
                </div>

                <button
                    type="submit"
                    disabled={isSubmitting}
                    className="bg-blue-500 text-white px-4 py-2 rounded"
                >
                    {isSubmitting ? "Updating..." : "Update Post"}
                </button>
            </form>
        </div>
    );
}
