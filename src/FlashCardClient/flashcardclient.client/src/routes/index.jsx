import { useAuth } from "../context/AuthContext";
import Button from "react-bootstrap/Button";

export default function Index() {
  const { isAuthenticated, login } = useAuth();

  return isAuthenticated ? (
    <div className="mt-5 text-center">
      <h4 className="text-secondary">No deck selected</h4>
      <div className="mt-3">Create or select one from the left panel.</div>
    </div>
  ) : (
    <div className="mt-5 text-center">
      <h4 className="text-primary">Welcome to AI-Powered Flash Card</h4>
      <h5 className="text-secondary">English learning flash card system</h5>
      <div className="mt-5">
        Click the button bellow to log in or register an account
      </div>
      <Button variant="secondary" className="mt-3" onClick={() => login()}>
          Log In or Register
        </Button>
    </div>
  );
}
