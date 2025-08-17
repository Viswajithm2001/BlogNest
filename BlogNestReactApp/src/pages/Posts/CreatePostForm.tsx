import React, { useState } from "react";
import { useForm } from "react-hook-form";
import { createPost } from "../../services/post";
import { useNavigate } from "react-router-dom";

type CreatePostFormData = {
    title: string;
    content: string;
    tags: string;
    image?: FileList; // change to FileList
};

export default function CreatePostForm() {
    const {
        register,
        handleSubmit,
        formState: { isSubmitting, errors },
    } = useForm<CreatePostFormData>();

    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();

    const onSubmit = async (data: CreatePostFormData) => {
        try {
            const tagsArray = data.tags
                .split(",")
                .map(tag => tag.trim())
                .filter(tag => tag.length > 0);

            const imageFile = data.image?.[0]; // pick first file
            await createPost(data.title, data.content, tagsArray);
            navigate("/posts");
        } catch (err: any) {
            setError(err.response?.data?.message || "Failed to create post");
        }
    };

    return (
        <div className="max-w-md mx-auto mt-10 p-6 bg-white shadow rounded">
            <h2 className="text-2xl font-bold mb-4">Create Post</h2>

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

                <div>
                    <label className="block mb-1">Image</label>
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
                    {isSubmitting ? "Creating..." : "Create Post"}
                </button>
            </form>
        </div>
    );
}
