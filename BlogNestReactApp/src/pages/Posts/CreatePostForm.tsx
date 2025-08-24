import React, { useState } from "react";
import { useForm } from "react-hook-form";
import { createPost } from "../../services/post";
import { useNavigate } from "react-router-dom";

type CreatePostFormData = {
    title: string;
    content: string;
    tags: string;
    image?: FileList; // keep for future use if you handle images
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

            // Currently ignoring image, backend doesn't handle it yet
            await createPost({
                title: data.title,
                content: data.content,
                tags: tagsArray,
            });

            navigate("/posts");
        } catch (err: any) {
            setError(err.response?.data?.message || "Failed to create post");
        }
    };

    return (
        <div className="max-w-md mx-auto mt-10 p-6 bg-white shadow rounded">
            <h2 className="text-2xl font-bold mb-4 text-center">Create Post</h2>

            {error && <p className="text-red-500 mb-3 text-center">{error}</p>}

            <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
                <div>
                    <label className="block mb-1 font-medium">Title</label>
                    <input
                        {...register("title", { required: "Title is required" })}
                        className="w-full border px-3 py-2 rounded focus:outline-none focus:ring-2 focus:ring-indigo-400"
                        placeholder="Enter post title"
                    />
                    {errors.title && <p className="text-red-500">{errors.title.message}</p>}
                </div>

                <div>
                    <label className="block mb-1 font-medium">Content</label>
                    <textarea
                        {...register("content", { required: "Content is required" })}
                        className="w-full border px-3 py-2 rounded focus:outline-none focus:ring-2 focus:ring-indigo-400 min-h-[150px]"
                        placeholder="Write your post here..."
                    />
                    {errors.content && <p className="text-red-500">{errors.content.message}</p>}
                </div>

                <div>
                    <label className="block mb-1 font-medium">Tags (comma separated)</label>
                    <input
                        {...register("tags")}
                        className="w-full border px-3 py-2 rounded focus:outline-none focus:ring-2 focus:ring-indigo-400"
                        placeholder="e.g. tech, react, dotnet"
                    />
                </div>

                {/* <div>
                    <label className="block mb-1 font-medium">Image (optional)</label>
                    <input
                        type="file"
                        accept="image/*"
                        {...register("image")}
                        className="w-full border px-3 py-2 rounded focus:outline-none focus:ring-2 focus:ring-indigo-400"
                    />
                </div> */}

                <button
                    type="submit"
                    disabled={isSubmitting}
                    className={`w-full py-2 text-white rounded ${
                        isSubmitting ? "bg-gray-400" : "bg-emerald-600 hover:bg-emerald-700"
                    } transition`}
                >
                    {isSubmitting ? "Creating..." : "Create Post"}
                </button>
            </form>
        </div>
    );
}
