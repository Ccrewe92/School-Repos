SET SERVEROUTPUT ON;

DECLARE
    -- Constants
    C_PRESIDENT_TITLE CONSTANT VARCHAR2(50) := 'PRESIDENT';
    C_HALF CONSTANT NUMBER(2,2) := 0.50;
    C_QUARTER_LESS CONSTANT NUMBER(2,2) := 0.25;
    C_SALARY_THRESHOLD CONSTANT NUMBER := 100;
    C_RAISE_PERCENT CONSTANT NUMBER(2,2) := 0.10;
    C_COMMISSION_THRESHOLD CONSTANT NUMBER(2,2) := 0.22;

    -- Variables
    v_avg_salary NUMBER;
    v_president_salary NUMBER;
    v_lowest_commission NUMBER;

    --counter to let the user know how many times these rules were applied and to which employees. 
    v_rule1_count NUMBER := 0;
    v_rule2_count NUMBER := 0;
    v_rule3_count NUMBER := 0;
    
    CURSOR emp_cur IS
        SELECT empno, sal, comm, job, deptno
        FROM emp;
    
    v_emp_rec emp_cur%ROWTYPE;

BEGIN
    -- Retrieve the average Employee salary and president's salary
    SELECT AVG(sal) INTO v_avg_salary FROM emp;
    SELECT sal INTO v_president_salary FROM emp WHERE job = C_PRESIDENT_TITLE;
    
    DBMS_OUTPUT.PUT_LINE('-----------------------------------------');
    DBMS_OUTPUT.PUT_LINE('Average Salary: ' || v_avg_salary);
    DBMS_OUTPUT.PUT_LINE('President Salary: ' || v_president_salary);
    DBMS_OUTPUT.PUT_LINE('-----------------------------------------');

    -- Loop through employees
    OPEN emp_cur;
    LOOP
        FETCH emp_cur INTO v_emp_rec;
        EXIT WHEN emp_cur%NOTFOUND;

        DBMS_OUTPUT.PUT_LINE('Processing employee#: ' || v_emp_rec.empno);

        -- Rule 1: Employee Salary vs President Salary: Employee Salary must be < Presidents Salary
        IF v_emp_rec.sal > v_president_salary THEN
            v_rule1_count := v_rule1_count + 1;
            DBMS_OUTPUT.PUT_LINE('Applying Rule 1 for employee#: ' || v_emp_rec.empno);
            v_emp_rec.sal := LEAST(v_emp_rec.sal * C_HALF, v_president_salary * (1 - C_QUARTER_LESS));
            DBMS_OUTPUT.PUT_LINE('Salary After Cut: ' || v_emp_rec.sal);
        END IF;

        -- Rule 2: Employee Salary vs Company Average Salary: If the employee salary is < $100 They can get a raise.
        IF v_emp_rec.sal < C_SALARY_THRESHOLD AND v_avg_salary > (v_emp_rec.sal * (1 + C_RAISE_PERCENT)) THEN
             v_rule2_count := v_rule2_count + 1;
            DBMS_OUTPUT.PUT_LINE('Applying Rule 2 for employee#: ' || v_emp_rec.empno);
            v_emp_rec.sal := v_emp_rec.sal * (1 + C_RAISE_PERCENT);
            DBMS_OUTPUT.PUT_LINE('Salary After Raise: ' || v_emp_rec.sal);
        END IF;

        -- Rule 3: Employee commission vs Department Commission: Employee making >22% of their original salary, Their commission is reduced.
        IF v_emp_rec.comm IS NOT NULL AND v_emp_rec.comm > 0 AND v_emp_rec.comm > v_emp_rec.sal * C_COMMISSION_THRESHOLD THEN
            v_rule3_count := v_rule3_count + 1;
            SELECT MIN(comm)
            INTO v_lowest_commission
            FROM emp
            WHERE deptno = v_emp_rec.deptno AND comm IS NOT NULL AND comm > 0;

            DBMS_OUTPUT.PUT_LINE('Applying Rule 3 for employee#: ' || v_emp_rec.empno);
            DBMS_OUTPUT.PUT_LINE('Lowest Commission in Department ' || v_emp_rec.deptno || ': ' || v_lowest_commission);
            v_emp_rec.comm := v_lowest_commission;
        END IF;

        -- Update the employee record
        UPDATE emp
        SET sal = v_emp_rec.sal, comm = v_emp_rec.comm
        WHERE empno = v_emp_rec.empno;


    END LOOP;
    CLOSE emp_cur;

    -- Rules applied
    DBMS_OUTPUT.PUT_LINE('-----------------------------------------');
    IF v_rule1_count = 0 THEN
       DBMS_OUTPUT.PUT_LINE('No employees make more than the president');
    ELSE
        DBMS_OUTPUT.PUT_LINE('Rule 1 was applied to ' || v_rule1_count || ' employees.');
    END IF;

    IF v_rule2_count = 0 THEN
       DBMS_OUTPUT.PUT_LINE('No employees makes less than $100');
    ELSE
        DBMS_OUTPUT.PUT_LINE('Rule 2 was applied to ' || v_rule2_count || ' employees.');
    END IF;

    IF v_rule3_count = 0 THEN
        DBMS_OUTPUT.PUT_LINE('No Employees make more than 22% commission');
    ELSE
        DBMS_OUTPUT.PUT_LINE('Rule 3 was applied to ' || v_rule3_count || ' employees.');
    END IF;

    COMMIT;

EXCEPTION
    WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE('Error: ' || SQLERRM);
        ROLLBACK;
END;
/
