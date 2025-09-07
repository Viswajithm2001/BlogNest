import { resetpwd, type ResetPassword } from "../../services/auth";
import { useForm } from "react-hook-form";
import { useNavigate } from "react-router-dom";

export default function ResetPassword() {
    const { register, handleSubmit, formState: { errors } } = useForm<ResetPassword>();
    const navigate = useNavigate();

    const onSubmit = async (data: ResetPassword) => {
        // Frontend password match check
        if (data.newPassword !== data.confirmPassword) {
            alert("Passwords do not match");
            return;
        }

        try {
            await resetpwd(data);
            alert("✅ Password reset successfully!");
            navigate("/login");
        } catch (err: any) {
            alert(err.response?.data?.message || "Password reset failed");
        }
    }

    return (
        <div className="flex items-center justify-center min-h-screen bg-gray-100">
            <div className="w-full max-w-md p-6 bg-white rounded-2xl shadow-lg">
                <h1 className="text-2xl font-bold text-center mb-6">Reset Password</h1>
                <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
                    <div>
                        <label className="block mb-1 text-sm font-medium">Email</label>
                        <input
                            type="email"
                            {...register("email", { required: "Email is required" })}
                            className="w-full border px-3 py-2 rounded mb-2"
                            placeholder="Email"
                        />
                        {errors.email && <p className="text-red-500 text-sm mb-2">{errors.email.message}</p>}

                        <label className="block mb-1 text-sm font-medium">New Password</label>
                        <input
                            type="password"
                            {...register("newPassword", { required: "Password is required" })}
                            className="w-full border px-3 py-2 rounded mb-2"
                            placeholder="New Password"
                        />
                        {errors.newPassword && <p className="text-red-500 text-sm mb-2">{errors.newPassword.message}</p>}

                        <label className="block mb-1 text-sm font-medium">Confirm Password</label>
                        <input
                            type="password"
                            {...register("confirmPassword", { required: "Confirm Password is required" })}
                            className="w-full border px-3 py-2 rounded mb-2"
                            placeholder="Confirm Password"
                        />
                        {errors.confirmPassword && <p className="text-red-500 text-sm mb-2">{errors.confirmPassword.message}</p>}

                        <button
                            type="submit"
                            className="w-full py-2 px-4 bg-blue-600 hover:bg-blue-700 text-white rounded-lg mt-2"
                        >
                            Submit
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
}
